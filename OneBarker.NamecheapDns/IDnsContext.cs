using System.Xml;
using OneBarker.NamecheapDns.Models;

namespace OneBarker.NamecheapDns;

/// <summary>
/// A DNS context.
/// </summary>
public interface IDnsContext
{
    /// <summary>
    /// Executes an API command and returns an XmlElement containing the response.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="parameters">The parameters for the command.</param>
    /// <returns>Returns an XmlElement.</returns>
    public XmlElement ExecuteApiCommand(string command, params KeyValuePair<string, string>[] parameters);

    /// <summary>
    /// Reloads the registered domains, any changes are discarded.
    /// </summary>
    public void Reload();
    
    /// <summary>
    /// The registered domains.
    /// </summary>
    public IReadOnlyList<IRegisteredDomain> RegisteredDomains { get; }
    
    /// <summary>
    /// Top level domain extensions known to the API.  Each one will start with a period (eg - ".com").
    /// </summary>
    public IReadOnlyList<string> TopLevelDomains { get; }
}
