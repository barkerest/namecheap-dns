namespace OneBarker.NamecheapDns.Exceptions;

/// <summary>
/// An exception thrown by the NamecheapDns library.
/// </summary>
public abstract class NamecheapDnsException : ApplicationException
{
    protected NamecheapDnsException(string message)
        : base(message)
    {
        
    }
}

