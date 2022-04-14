using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// A collection of host records with the same name.
/// </summary>
public class HostRecordSet : IHostRecordSet, IValidatableObject
{
    private readonly ICollection<IHostRecord>                         _allHostRecords;
    private          HostRecordCollection<HostIPv4Record>?            _pv4Addresses;
    private          HostRecordCollection<HostIPv6Record>?            _pv6Addresses;
    private          HostRecordCollection<HostAliasRecord>?           _aliases;
    private          HostRecordCollection<HostCAAuthRecord>?          _certificateAuthorities;
    private          HostRecordCollection<HostMailRecord>?            _mailServers;
    private          HostRecordCollection<HostNameserverRecord>?      _nameservers;
    private          HostRecordCollection<HostTextRecord>?            _textValues;
    private          HostRecordAccessor<HostCNameRecord>?             _cname;
    private          HostRecordAccessor<HostMailEasyRecord>?          _mailAddress;
    private          HostRecordAccessor<HostMaskedRedirectRecord>?    _masked;
    private          HostRecordAccessor<HostUnmaskedRedirectRecord>?  _unmasked;
    private          HostRecordAccessor<HostPermanentRedirectRecord>? _permanent;

    public HostRecordSet()
        : this(new List<IHostRecord>(), "")
    {
    }

    public HostRecordSet(string name)
        : this(new List<IHostRecord>(), name)
    {
    }

    public HostRecordSet(ICollection<IHostRecord> allHostRecords, string name)
    {
        _allHostRecords = allHostRecords;
        Name            = name;
    }


    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public IHostRecordCollection IPv4Addresses
        => _pv4Addresses ??= new HostRecordCollection<HostIPv4Record>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordCollection IPv6Addresses
        => _pv6Addresses ??= new HostRecordCollection<HostIPv6Record>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordCollection Aliases
        => _aliases ??= new HostRecordCollection<HostAliasRecord>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordAccessor CanonicalName
        => _cname ??= new HostRecordAccessor<HostCNameRecord>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordCollection CertificateAuthorities
        => _certificateAuthorities ??= new HostRecordCollection<HostCAAuthRecord>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordCollectionWithPreference MailServers
        => _mailServers ??= new HostRecordCollection<HostMailRecord>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordAccessor MailAddress
        => _mailAddress ??= new HostRecordAccessor<HostMailEasyRecord>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordCollection Nameservers
        => _nameservers ??= new HostRecordCollection<HostNameserverRecord>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordCollection TextValues
        => _textValues ??= new HostRecordCollection<HostTextRecord>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordAccessor MaskedRedirect
        => _masked ??= new HostRecordAccessor<HostMaskedRedirectRecord>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordAccessor UnmaskedRedirect
        => _unmasked ??= new HostRecordAccessor<HostUnmaskedRedirectRecord>(_allHostRecords, Name);

    /// <inheritdoc />
    public IHostRecordAccessor PermanentRedirect
        => _permanent ??= new HostRecordAccessor<HostPermanentRedirectRecord>(_allHostRecords, Name);

    private IEnumerable<IHostRecord> GetHostRecords() => _allHostRecords.Where(x => x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));

    /// <inheritdoc />
    public void Clear()
    {
        var records = GetHostRecords().ToArray();
        foreach (var record in records)
        {
            _allHostRecords.Remove(record);
        }
    }

    private void Copy<T>(HostRecordAccessor<T>? _, IHostRecordAccessor to, IHostRecordAccessor from) where T : class, IHostRecord, new()
    {
        ((HostRecordAccessor<T>)to).CopyFrom(from);
    }

    private void Copy<T>(HostRecordCollection<T>? _, IHostRecordCollection to, IHostRecordCollection from) where T : class, IHostRecord, new()
    {
        ((HostRecordCollection<T>)to).CopyFrom(from);
    }
    
    internal void CopyFrom(IHostRecordSet other)
    {
        // same record set, no changes to make.
        if (ReferenceEquals(this, other)) return;
        
        // same backing array and name.
        if (other is HostRecordSet hrs &&
            ReferenceEquals(_allHostRecords, hrs._allHostRecords) &&
            Name.Equals(hrs.Name, StringComparison.OrdinalIgnoreCase)) return;

        Clear();

        Copy(_pv4Addresses, IPv4Addresses, other.IPv4Addresses);
        Copy(_pv6Addresses, IPv6Addresses, other.IPv6Addresses);
        Copy(_aliases, Aliases, other.Aliases);
        Copy(_cname, CanonicalName, other.CanonicalName);
        Copy(_certificateAuthorities, CertificateAuthorities, other.CertificateAuthorities);
        Copy(_mailServers, MailServers, other.MailServers);
        Copy(_mailAddress, MailAddress, other.MailAddress);
        Copy(_nameservers, Nameservers, other.Nameservers);
        Copy(_textValues, TextValues, other.TextValues);
        Copy(_unmasked, UnmaskedRedirect, other.UnmaskedRedirect);
        Copy(_masked, MaskedRedirect, other.MaskedRedirect);
        Copy(_permanent, PermanentRedirect, other.PermanentRedirect);
    }

    IEnumerable<IHostRecord> IHostRecordSet.ToHostRecords()
    {
        foreach (var record in IPv4Addresses.ToHostRecords()) yield return record;
        foreach (var record in IPv6Addresses.ToHostRecords()) yield return record;
        foreach (var record in Aliases.ToHostRecords()) yield return record;
        foreach (var record in CanonicalName.ToHostRecords()) yield return record;
        foreach (var record in CertificateAuthorities.ToHostRecords()) yield return record;
        foreach (var record in MailServers.ToHostRecords()) yield return record;
        foreach (var record in MailAddress.ToHostRecords()) yield return record;
        foreach (var record in Nameservers.ToHostRecords()) yield return record;
        foreach (var record in TextValues.ToHostRecords()) yield return record;
        foreach (var record in UnmaskedRedirect.ToHostRecords()) yield return record;
        foreach (var record in MaskedRedirect.ToHostRecords()) yield return record;
        foreach (var record in PermanentRedirect.ToHostRecords()) yield return record;
    }

    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        var records = GetHostRecords().ToArray();

        var countC    = records.Count(x => x.Type is RecordType.CNAME);
        var countNonC = records.Count(x => x.Type is not RecordType.CNAME);

        if (countC > 0 &&
            countNonC > 0)
        {
            yield return new ValidationResult($"cannot exist alongside other records", new[] { nameof(CanonicalName) });
        }

        var countA = records.Count(x => x.Type is RecordType.A or RecordType.AAAA or RecordType.ALIAS);

        var countM1 = records.Count(x => x.Type is RecordType.MX);
        var countM2 = records.Count(x => x.Type is RecordType.MXE);
        if (countM1 > 0 &&
            countM2 > 0)
        {
            yield return new ValidationResult($"cannot be set alongside {nameof(MailAddress)}", new[] { nameof(MailServers) });
            yield return new ValidationResult($"cannot be set alongside {nameof(MailServers)}", new[] { nameof(MailAddress) });
        }

        var ru = records.Count(x => x.Type is RecordType.URL);
        var rm = records.Count(x => x.Type is RecordType.FRAME);
        var rp = records.Count(x => x.Type is RecordType.URL301);

        if (ru > 0)
        {
            if (rm > 0) yield return new ValidationResult($"cannot be set alongside {nameof(MaskedRedirect)}", new[] { nameof(UnmaskedRedirect) });
            if (rp > 0) yield return new ValidationResult($"cannot be set alongside {nameof(PermanentRedirect)}", new[] { nameof(UnmaskedRedirect) });
            if (countA > 0) yield return new ValidationResult($"cannot be set alongside {nameof(IPv4Addresses)}, {nameof(IPv6Addresses)}, or {nameof(Aliases)}", new[] { nameof(UnmaskedRedirect) });
        }

        if (rm > 0)
        {
            if (ru > 0) yield return new ValidationResult($"cannot be set alongside {nameof(UnmaskedRedirect)}", new[] { nameof(MaskedRedirect) });
            if (rp > 0) yield return new ValidationResult($"cannot be set alongside {nameof(PermanentRedirect)}", new[] { nameof(MaskedRedirect) });
            if (countA > 0) yield return new ValidationResult($"cannot be set alongside {nameof(IPv4Addresses)}, {nameof(IPv6Addresses)}, or {nameof(Aliases)}", new[] { nameof(MaskedRedirect) });
        }

        if (rp > 0)
        {
            if (ru > 0) yield return new ValidationResult($"cannot be set alongside {nameof(UnmaskedRedirect)}", new[] { nameof(PermanentRedirect) });
            if (rm > 0) yield return new ValidationResult($"cannot be set alongside {nameof(MaskedRedirect)}", new[] { nameof(PermanentRedirect) });
            if (countA > 0) yield return new ValidationResult($"cannot be set alongside {nameof(IPv4Addresses)}, {nameof(IPv6Addresses)}, or {nameof(Aliases)}", new[] { nameof(PermanentRedirect) });
        }
        
    }
}
