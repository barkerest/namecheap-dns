using System.Xml;

namespace OneBarker.NamecheapDns.Exceptions;

/// <summary>
/// The XML response was missing a required element.
/// </summary>
public class MissingElementException : MissingDataException
{
    public MissingElementException(string elementName)
        : base($"The <{elementName}> element is missing.")
    {
        
    }

    public MissingElementException(string parentElementName, string elementName)
        : base($"The <{elementName}> is missing from the <{parentElementName}> element.")
    {
        
    }
}
