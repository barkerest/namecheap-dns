using System.Xml;
using OneBarker.NamecheapDns.Enums;
using OneBarker.NamecheapDns.Exceptions;
using OneBarker.NamecheapDns.Utility;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// A registered domain.
/// </summary>
public class RegisteredDomain : IRegisteredDomain
{
    private readonly ICollection<IHostRecord> _allHostRecords    = new List<IHostRecord>();
    private readonly bool                     _ownAllHostRecords = true;
    private readonly IDnsContext              _context;
    private          string[]?                _dnsServers;
    private          HostRecordDictionary?    _hostRecordDictionary;


    public RegisteredDomain()
    {
        _context = NullDnsContext.Instance;
    }

    public RegisteredDomain(ICollection<IHostRecord> allHostRecords, string domainName)
    {
        DomainName         = domainName;
        _context           = NullDnsContext.Instance;
        _allHostRecords    = allHostRecords;
        _ownAllHostRecords = false;
    }

    public RegisteredDomain(IDnsContext context, XmlElement element)
    {
        _context = context;

        element.RequireName("Domain");

        DomainName       = element.GetRequiredAttribute("Name");
        Expiration       = element.GetAttributeAsDateTime("Expires");
        IsExpired        = element.GetAttributeAsBoolean("IsExpired");
        AutoRenew        = element.GetAttributeAsBoolean("AutoRenew");
        UsesNamecheapDns = element.GetAttributeAsBoolean("IsOurDNS");
    }

    /// <inheritdoc />
    public string DomainName { get; } = "";

    /// <inheritdoc />
    public DateTime Expiration { get; } = DateTime.MinValue;

    /// <inheritdoc />
    public bool IsExpired { get; } = false;

    /// <inheritdoc />
    public bool AutoRenew { get; } = false;

    /// <inheritdoc />
    public bool UsesNamecheapDns { get; } = false;

    /// <inheritdoc />
    public bool Save()
    {
        if (!HostRecords.IsValid(out var errors))
        {
            SaveErrors = errors;
            return false;
        }
        
        var (sld, tld) = _context.SplitDomainName(DomainName);
        var data = new Dictionary<string, string>()
        {
            { "SLD", sld },
            { "TLD", tld }
        };

        var n = 1;
        foreach (var record in _allHostRecords)
        {
            var ns = n.ToString();
            data["HostName" + ns]   = record.Name;
            data["RecordType" + ns] = record.Type.ToString();
            data["Address" + ns]    = record.Value;
            data["TTL" + ns]        = record.TimeToLive.ToString();
            if (record.Preference > 0) data["MXPref" + ns] = record.Preference.ToString();
            n++;
        }

        try
        {
            var response = _context.ExecuteApiCommand(
                ApiCommand.SetDomainDnsHosts,
                data.ToArray()
            );

            if (response.GetChild(ApiCommand.SetDomainDnsHostsResult) is { } result)
            {
                var success =result.GetAttributeAsBoolean("IsSuccess");
                SaveErrors = success ? Array.Empty<string>() : new[] { "API: The save operation failed." };
                return success;
            }

            SaveErrors = new[] { "API: No result was returned from the API." };
            
            return false;
        }
        catch (NamecheapDnsException ex)
        {
            SaveErrors = new[] { "API: " + ex.Message };
            return false;
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<string> SaveErrors { get; private set; } = Array.Empty<string>();

    /// <inheritdoc />
    public void Reload()
    {
        if (!_ownAllHostRecords) return;    // do not touch host records we do not own

        _allHostRecords.Clear();
        _hostRecordDictionary?.Clear();
        
        var (sld, tld) = _context.SplitDomainName(DomainName);
        var response = _context.ExecuteApiCommand(
            ApiCommand.GetDomainDnsHosts,
            new KeyValuePair<string, string>("SLD", sld),
            new KeyValuePair<string, string>("TLD", tld)
        );
        
        if (response.GetChild(ApiCommand.GetDomainDnsHostsResult) is { } result)
        {
            foreach (var host in result.ChildNodes.OfType<XmlElement>().Where(x => x.Name.Equals("Host", StringComparison.OrdinalIgnoreCase)))
            {
                var name = host.GetRequiredAttribute("Name");
                var type = host.GetRequiredAttributeAsEnum<RecordType>("Type");
                var val = host.GetRequiredAttribute("Address");
                var pref = host.GetAttributeAsInt32("MXPref");
                var ttl  = host.GetAttributeAsInt32("TTL");

                IHostRecord record = type switch
                {
                    RecordType.A      => new HostIPv4Record(name, val, ttl),
                    RecordType.AAAA   => new HostIPv6Record(name, val, ttl),
                    RecordType.ALIAS  => new HostAliasRecord(name, val, ttl),
                    RecordType.CAA    => new HostCAAuthRecord(name, val, ttl),
                    RecordType.CNAME  => new HostCNameRecord(name, val, ttl),
                    RecordType.MX     => new HostMailRecord(name, val, ttl, pref),
                    RecordType.MXE    => new HostMailEasyRecord(name, val, ttl),
                    RecordType.NS     => new HostNameserverRecord(name, val, ttl),
                    RecordType.TXT    => new HostTextRecord(name, val, ttl),
                    RecordType.URL    => new HostUnmaskedRedirectRecord(name, val, ttl),
                    RecordType.URL301 => new HostPermanentRedirectRecord(name, val, ttl),
                    RecordType.FRAME  => new HostMaskedRedirectRecord(name, val, ttl),
                    _                 => throw new ArgumentOutOfRangeException()
                };

                _allHostRecords.Add(record);
            }
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<string> DnsServers
    {
        get
        {
            if (_dnsServers is null)
            {
                var (sld, tld) = _context.SplitDomainName(DomainName);
                var response = _context.ExecuteApiCommand(
                    ApiCommand.GetDomainDnsServers,
                    new KeyValuePair<string, string>("SLD", sld),
                    new KeyValuePair<string, string>("TLD", tld)
                );
                if (response.GetChild(ApiCommand.GetDomainDnsServersResult) is { } result)
                {
                    _dnsServers = result.ChildNodes
                                        .OfType<XmlElement>()
                                        .Where(x => x.Name.Equals("Nameserver", StringComparison.OrdinalIgnoreCase))
                                        .Select(x => x.GetContent().Trim())
                                        .ToArray();
                }
                else
                {
                    _dnsServers = Array.Empty<string>();
                }
            }

            return _dnsServers;
        }
    }

    /// <inheritdoc />
    public IHostRecordDictionary HostRecords
    {
        get
        {
            if (_hostRecordDictionary is null)
            {
                Reload();
                _hostRecordDictionary = new HostRecordDictionary(_allHostRecords);
            }

            return _hostRecordDictionary;
        }
    }

    /// <inheritdoc />
    IEnumerable<IHostRecord> IRegisteredDomain.ToHostRecords()
        => _allHostRecords.OrderBy(x => x.Name).ThenBy(x => x.Type).ThenBy(x => x.Preference).ThenBy(x => x.Value);
}
