using OneBarker.NamecheapDns.Models;

namespace OneBarker.NamecheapDns.Utility;

public static class ValidationExtensions
{
    /// <summary>
    /// Validates the object and returns any errors encountered.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static bool IsValid(this object self, out string[] errors)
        => ObjectValidator.Validate(self, out errors);
}
