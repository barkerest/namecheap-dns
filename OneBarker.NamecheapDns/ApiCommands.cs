namespace OneBarker.NamecheapDns;

internal static class ApiCommand
{
    public static readonly string GetDomainList               = "namecheap.domains.getList";
    public static readonly string GetDomainListResult         = "DomainGetListResult";
    public static readonly string GetTopLevelDomainList       = "namecheap.domains.getTldList";
    public static readonly string GetTopLevelDomainListResult = "Tlds";
    public static readonly string GetDomainDnsServers         = "namecheap.domains.dns.getList";
    public static readonly string GetDomainDnsServersResult   = "DomainDNSGetListResult";
    public static readonly string GetDomainDnsHosts           = "namecheap.domains.dns.getHosts";
    public static readonly string GetDomainDnsHostsResult     = "DomainDNSGetHostsResult";
    public static readonly string SetDomainDnsHosts           = "namecheap.domains.dns.setHosts";
    public static readonly string SetDomainDnsHostsResult     = "DomainDNSSetHostsResult";
}
