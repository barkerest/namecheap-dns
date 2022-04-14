namespace OneBarker.NamecheapDns.Exceptions;

public class ApiErrorException : NamecheapDnsException
{
    public ApiErrorException(IEnumerable<(int number, string message)> errors)
        : base("API Errors:\n  " + string.Join("\n  ", errors.Select(x => $"[{x.number}] {x.message}")))
    {
        
    }
}
