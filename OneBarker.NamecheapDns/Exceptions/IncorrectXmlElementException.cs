namespace OneBarker.NamecheapDns.Exceptions;

/// <summary>
/// Encountered an element with the wrong name.
/// </summary>
public class IncorrectXmlElementException : InvalidDataException
{
    public IncorrectXmlElementException(string expectedName, string name)
        : base($"Expected <{expectedName}> element but found <{name}> instead.")
    {
        
    }
}
