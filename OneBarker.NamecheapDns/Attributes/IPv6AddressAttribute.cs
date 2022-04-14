using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;

namespace OneBarker.NamecheapDns.Attributes;

/// <summary>
/// The value must be a valid IPv6 address.
/// </summary>
public class IPv6AddressAttribute : ValidationAttribute
{
    public IPv6AddressAttribute()
        : base("The {0} field must be a valid IPv6 address.")
    {
        
    }
    
    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        if (value is IPAddress ip) return ip.AddressFamily == AddressFamily.InterNetworkV6;
        if (value is string s &&
            IPAddress.TryParse(s, out var ip2))
        {
            return ip2.AddressFamily == AddressFamily.InterNetworkV6;
        }
        return false;
    }
}
