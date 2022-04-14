namespace OneBarker.NamecheapDns.Utility;

public static class DnsContextExtensions
{
    /// <summary>
    /// Splits the domain name into the second-level and top-level portions.
    /// </summary>
    /// <param name="context">The context used to provide the top-level domains.</param>
    /// <param name="domainName">The domain name to split.</param>
    /// <returns></returns>
    public static (string sld, string tld) SplitDomainName(this IDnsContext context, string domainName)
    {
        if (string.IsNullOrWhiteSpace(domainName)) return ("", "");
        if (!domainName.Contains('.')) return (domainName, "");

        foreach (var tld in context.TopLevelDomains.OrderByDescending(x => x.Length).ThenBy(x => x))
        {
            if (domainName.EndsWith(tld, StringComparison.OrdinalIgnoreCase))
            {
                return (domainName[..^tld.Length], tld[1..]);
            }
        }

        var x = domainName.LastIndexOf('.');
        return (domainName[..x], domainName[(x + 1)..]);
    }
    
    
}
