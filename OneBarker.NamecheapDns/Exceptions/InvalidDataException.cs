namespace OneBarker.NamecheapDns.Exceptions;

/// <summary>
/// The data returned from the API is invalid.
/// </summary>
public abstract class InvalidDataException : NamecheapDnsException
{
    protected InvalidDataException(string message)
        : base(message)
    {
    }
}
