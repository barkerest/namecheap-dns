namespace OneBarker.NamecheapDns.Enums;

/// <summary>
/// The type of CAA record.
/// </summary>
public enum CertificateAuthorityAuthTag
{
    /// <summary>
    /// Specifies the certification authority that is authorized to issue a certificate for the domain name or subdomain record used in the title. 
    /// </summary>
    Issue,
    
    /// <summary>
    /// Specifies the certification authority that is allowed to issue a wildcard certificate for the domain name or subdomain record used in the title.
    /// </summary>
    IssueWild,
    
    /// <summary>
    /// Specifies the e-mail address or URL (compliant with RFC 5070) a CA should use to notify a client if any issuance policy violation spotted by this CA.
    /// </summary>
    IODEF
}
