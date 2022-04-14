namespace OneBarker.NamecheapDns.Models;

public interface IHostRecordDictionary : IDictionary<string, IHostRecordSet>
{
    /// <summary>
    /// Checks the contents of the dictionary for validity.
    /// </summary>
    /// <param name="errors">Returns any errors found.</param>
    /// <returns>Returns true if all of the host records are valid.  Returns false if any errors were encountered.</returns>
    public bool IsValid(out string[] errors);
}
