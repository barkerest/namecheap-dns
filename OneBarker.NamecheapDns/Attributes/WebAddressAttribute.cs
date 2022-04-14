using System.ComponentModel.DataAnnotations;

namespace OneBarker.NamecheapDns.Attributes;

/// <summary>
/// The value must be an HTTP(S) web address.
/// </summary>
public class WebAddressAttribute : ValidationAttribute
{
    public WebAddressAttribute()
        : base("The {0} field must be an HTTP web address.")
    {
        
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        if (value is Uri uri) return uri.Scheme is "http" or "https";
        if (value is string s &&
            Uri.TryCreate(s, UriKind.Absolute, out var uri2))
        {
            return uri2.Scheme is "http" or "https";
        }

        return false;
    }
}
