using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// A DNS host record.
/// </summary>
public interface IHostRecord
{
    /// <summary>
    /// The record type.
    /// </summary>
    public RecordType Type { get; }

    /// <summary>
    /// The name for the host record.
    /// </summary>
    /// <remarks>
    /// *   - (all)
    /// @   - (none)
    /// www - www
    /// </remarks>
    public string Name { get; }
    
    /// <summary>
    /// The value for this record.
    /// </summary>
    /// <remarks>
    /// This is the full value sent to and received from the Namecheap API.
    /// For certain record types this may be broken down into additional properties (eg - CAA => Flag, Tag, & Uri)
    /// </remarks>
    public string Value { get; }
    
    /// <summary>
    /// Number of seconds that this host record is valid before clients request a refresh.
    /// </summary>
    /// <remarks>
    /// Lower values increase the load on the nameservers.
    /// Higher values increase the time needed to propagate changes.
    /// </remarks>
    public int TimeToLive { get; }
    
    /// <summary>
    /// Preference for this record.
    /// </summary>
    /// <remarks>
    /// Only used by MX records.
    /// The lower the value the higher the precedence.
    /// </remarks>
    public int Preference { get; }

    /// <summary>
    /// Returns a host record with the values set to the provided values.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="timeToLive"></param>
    /// <param name="preference"></param>
    /// <returns>Returns a host record of the same type with the specified values set.</returns>
    public IHostRecord Create(string name, string value, int timeToLive, int preference);

}
