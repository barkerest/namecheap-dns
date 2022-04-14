namespace OneBarker.NamecheapDns.Enums;

/// <summary>
/// The DNS record types supported by Namecheap API.
/// </summary>
public enum RecordType
{
    /// <summary>
    /// Address record (IPv4)
    /// </summary>
    A,
    
    /// <summary>
    /// Address record (IPv6) 
    /// </summary>
    AAAA,
    
    /// <summary>
    /// (Namcheap Specific) Virtual address record that updates from another address record automatically.
    /// </summary>
    ALIAS,
    
    /// <summary>
    /// Certification authority authorization.
    /// </summary>
    CAA,
    
    /// <summary>
    /// Canonical name record.
    /// </summary>
    CNAME,
    
    /// <summary>
    /// Mail exchange record.
    /// </summary>
    MX,
    
    /// <summary>
    /// (Namecheap Specific) Easy mail exchange record (forward to IP).
    /// </summary>
    MXE,
    
    /// <summary>
    /// Name server record.
    /// </summary>
    NS,
    
    /// <summary>
    /// Text record.
    /// </summary>
    TXT,
    
    /// <summary>
    /// (Namecheap Specific) URL redirect - unmasked. 
    /// </summary>
    URL,
    
    /// <summary>
    /// (Namecheap Specific) URL redirect - unmasked, permanent
    /// </summary>
    URL301,
    
    /// <summary>
    /// (Namecheap Specific) URL redirect - masked.
    /// </summary>
    FRAME
}
