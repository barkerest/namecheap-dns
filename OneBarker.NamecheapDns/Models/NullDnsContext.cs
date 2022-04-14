using System.Xml;

namespace OneBarker.NamecheapDns.Models;

internal class NullDnsContext : IDnsContext
{
    private readonly XmlDocument _doc;

    private NullDnsContext()
    {
        _doc = new XmlDocument();
        _doc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\">\n<CommandResponse />\n");
    }

    public static readonly NullDnsContext Instance = new();
    
    /// <inheritdoc />
    public XmlElement ExecuteApiCommand(string command, params KeyValuePair<string, string>[] parameters)
    {
        return _doc.DocumentElement!;
    }

    /// <inheritdoc />
    public void Reload()
    {
        
    }

    /// <inheritdoc />
    public IReadOnlyList<IRegisteredDomain> RegisteredDomains { get; } = Array.Empty<IRegisteredDomain>();

    /// <inheritdoc />
    public IReadOnlyList<string> TopLevelDomains { get; } = new[] { ".com", ".net", ".org", ".edu", ".gov" };
}
