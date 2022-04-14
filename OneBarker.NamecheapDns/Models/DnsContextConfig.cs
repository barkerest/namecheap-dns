using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapDns.Attributes;
using OneBarker.NamecheapDns.Utility;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// The configuration for a DNS context.
/// </summary>
public class DnsContextConfig : IDnsContextConfig
{
    /// <summary>
    /// The sandbox host, for testing.
    /// </summary>
    public static readonly string SandboxHost = "api.sandbox.namecheap.com";

    /// <summary>
    /// The production host.
    /// </summary>
    public static readonly string ProductionHost = "api.namecheap.com";

    /// <summary>
    /// Create a new configuration.
    /// </summary>
    /// <param name="apiHost">The API host to use.</param>
    /// <param name="apiUser">The API user to login with.</param>
    /// <param name="apiKey">The API key to login with.</param>
    /// <param name="clientIp">The client IP (defaults to the current system's public IP).</param>
    /// <param name="userName">The API user to run as (defaults to the user logging in).</param>
    public DnsContextConfig(string apiHost, string apiUser, string apiKey, string? clientIp = null, string? userName = null)
    {
        ApiHost  = apiHost;
        ApiUser  = apiUser;
        ApiKey   = apiKey;
        ClientIP = clientIp ?? this.FindPublicIP();
        UserName = userName ?? apiUser;
    }

    /// <inheritdoc />
    [Required, StringLength(255), WebAddress]
    public string ApiHost { get; }

    /// <inheritdoc />
    [Required, StringLength(20)]
    public string ApiUser { get; }

    /// <inheritdoc />
    [Required, StringLength(50)]
    public string ApiKey { get; }

    /// <inheritdoc />
    [Required, StringLength(20)]
    public string UserName { get; }

    /// <inheritdoc />
    [Required, StringLength(15), IPv4Address]
    public string ClientIP { get; }
}
