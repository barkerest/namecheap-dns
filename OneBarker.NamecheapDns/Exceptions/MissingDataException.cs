namespace OneBarker.NamecheapDns.Exceptions;

/// <summary>
/// The data returned from the API is missing data.
/// </summary>
public abstract class MissingDataException : NamecheapDnsException
{
    protected MissingDataException(string message)
        : base(message)
    {
    }
}
