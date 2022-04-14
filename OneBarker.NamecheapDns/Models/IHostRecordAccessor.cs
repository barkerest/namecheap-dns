namespace OneBarker.NamecheapDns.Models;

public interface IHostRecordAccessor : IHostRecord
{
    /// <summary>
    /// Change the TTL value for all accessible records.
    /// </summary>
    /// <param name="newTimeToLive"></param>
    public void ChangeTimeToLive(int newTimeToLive);
    
    /// <summary>
    /// Replace any existing value with the supplied value.
    /// </summary>
    /// <param name="newValue"></param>
    public void SetValue(string newValue);

    /// <summary>
    /// Clears any existing value.
    /// </summary>
    public void ClearValue();

    /// <summary>
    /// Returns a list of host records from this collection.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IHostRecord> ToHostRecords();
}
