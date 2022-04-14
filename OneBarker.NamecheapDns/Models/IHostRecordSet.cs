namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// The interface used by a record set for a named host.
/// </summary>
public interface IHostRecordSet
{
    /// <summary>
    /// The name for this record set.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The A records under this name.
    /// </summary>
    public IHostRecordCollection IPv4Addresses { get; }

    /// <summary>
    /// The AAAA records under this name.
    /// </summary>
    public IHostRecordCollection IPv6Addresses { get; }

    /// <summary>
    /// The ALIAS records under this name.
    /// </summary>
    public IHostRecordCollection Aliases { get; }

    /// <summary>
    /// The CNAME record for this name.
    /// </summary>
    public IHostRecordAccessor CanonicalName { get; }

    /// <summary>
    /// The CAA records for this name.
    /// </summary>
    public IHostRecordCollection CertificateAuthorities { get; }

    /// <summary>
    /// The MX records for this name (cannot be used with MXE records).
    /// </summary>
    public IHostRecordCollectionWithPreference MailServers { get; }

    /// <summary>
    /// The MXE record for this name (cannot be used with MX records).
    /// </summary>
    public IHostRecordAccessor MailAddress { get; }

    /// <summary>
    /// The NS records for this name.
    /// </summary>
    public IHostRecordCollection Nameservers { get; }

    /// <summary>
    /// The TXT records for this name.
    /// </summary>
    public IHostRecordCollection TextValues { get; }

    /// <summary>
    /// The FRAME masked redirect value (cannot be used with unmasked or permanent redirects).
    /// </summary>
    public IHostRecordAccessor MaskedRedirect { get; }

    /// <summary>
    /// The URL unmasked redirect value (cannot be used with masked or permanent redirects).
    /// </summary>
    public IHostRecordAccessor UnmaskedRedirect { get; }

    /// <summary>
    /// The URL301 permanent redirect value (cannot be used with masked or unmasked redirects).
    /// </summary>
    public IHostRecordAccessor PermanentRedirect { get; }

    /// <summary>
    /// Clears all records from this record set.
    /// </summary>
    public void Clear();
    
    /// <summary>
    /// Returns a list of host records from this set.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IHostRecord> ToHostRecords();
}
