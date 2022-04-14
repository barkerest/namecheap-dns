namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// The configuration for a DnsContext.
/// </summary>
public interface IDnsContextConfig
{
    /// <summary>
    /// The host to connect to.
    /// </summary>
    public string ApiHost { get; }
    
    /// <summary>
    /// The API user to login with.
    /// </summary>
    public string ApiUser { get; }
    
    /// <summary>
    /// The API key to login with.
    /// </summary>
    public string ApiKey { get; }
    
    /// <summary>
    /// The API user to run as.
    /// </summary>
    public string UserName { get; }
    
    /// <summary>
    /// The client IP address.
    /// </summary>
    public string ClientIP { get; }
}
