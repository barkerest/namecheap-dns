using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;

namespace OneBarker.NamecheapDns.Attributes;

/// <summary>
/// The value must be a valid IPv4 address.
/// </summary>
public class IPv4AddressAttribute : ValidationAttribute
{
    public IPv4AddressAttribute()
        : base("The {0} field must be a valid IPv4 address.")
    {
        
    }
    
    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        if (value is IPAddress ip) return ip.AddressFamily == AddressFamily.InterNetwork;
        if (value is string s &&
            IPAddress.TryParse(s, out var ip2))
        {
            return ip2.AddressFamily == AddressFamily.InterNetwork;
        }
        return false;
    }
}
