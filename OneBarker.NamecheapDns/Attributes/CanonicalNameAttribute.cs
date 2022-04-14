using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OneBarker.NamecheapDns.Attributes;

/// <summary>
/// The value must be a host canonical name (eg - something.someplace.)
/// </summary>
public class CanonicalNameAttribute : ValidationAttribute
{
    /// <summary>
    /// A simple pattern for validating a canonical name.
    /// </summary>
    public static readonly Regex CanonicalNamePattern = new(@"^[a-z0-9]([a-z0-9_-]*[a-z0-9])?(?:\.[a-z0-9]([a-z0-9_-]*[a-z0-9])?)*\.?$", RegexOptions.IgnoreCase);
    
    public CanonicalNameAttribute()
        : base("The {0} field must be a valid canonical name.")
    {
        
    }

    /// <summary>
    /// Indicates whether a trailing period is required.
    /// </summary>
    public bool RequireTrailingPeriod { get; set; } = false;

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        if (value is not string s) return false;
        if (RequireTrailingPeriod && !s.EndsWith('.')) return false;
        return CanonicalNamePattern.IsMatch(s);
    }
}
