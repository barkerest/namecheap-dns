namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// A registered domain within Namecheap.
/// </summary>
public interface IRegisteredDomain
{
    /// <summary>
    /// The registered domain name.
    /// </summary>
    public string DomainName { get; }

    /// <summary>
    /// The expiration date for the domain registration.
    /// </summary>
    public DateTime Expiration { get; }
    
    /// <summary>
    /// Indicates whether the domain is expired.
    /// </summary>
    public bool IsExpired { get; }
    
    /// <summary>
    /// Indicates whether the domain is setup for automatic renewal.
    /// </summary>
    public bool AutoRenew { get; }
    
    /// <summary>
    /// Indicates whether the domain uses Namecheap DNS servers.
    /// </summary>
    public bool UsesNamecheapDns { get; }
    
    /// <summary>
    /// Saves the host records to Namecheap.
    /// </summary>
    /// <returns>Returns true on success.</returns>
    /// <remarks>
    /// Will always fail if the domain does not use Namecheap DNS servers.
    /// </remarks>
    public bool Save();
    
    /// <summary>
    /// Contains any errors encountered when attempting to save the host records.
    /// </summary>
    public IReadOnlyList<string> SaveErrors { get; }

    /// <summary>
    /// Reloads the host records from Namecheap, any changes are discarded.
    /// </summary>
    public void Reload();

    /// <summary>
    /// Returns the DNS servers associated with this domain.
    /// </summary>
    public IReadOnlyList<string> DnsServers { get; }
    
    /// <summary>
    /// Gets the host records in a manageable format.
    /// </summary>
    public IHostRecordDictionary HostRecords { get; }

    /// <summary>
    /// Returns a list of host records for this domain.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IHostRecord> ToHostRecords();
}
