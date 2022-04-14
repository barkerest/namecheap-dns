namespace OneBarker.NamecheapDns.Exceptions;

public class MissingAttributeException : MissingDataException
{
    public MissingAttributeException(string elementName, string attributeName)
        : base($"The <{elementName}> element is missing the '{attributeName}' attribute.")
    {
        
    }
}
