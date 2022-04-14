using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapDns.Attributes;
using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// [NS] A nameserver record.
/// </summary>
public class HostNameserverRecord : IHostRecord
{
    public HostNameserverRecord()
    {
    }

    public HostNameserverRecord(string name, string value, int timeToLive)
    {
        Name       = name;
        Value      = value;
        TimeToLive = timeToLive;
    }

    /// <inheritdoc />
    public RecordType Type => RecordType.NS;

    /// <inheritdoc />
    [Required, StringLength(63)]
    public string Name { get; } = "";

    /// <summary>
    /// The canonical name for this record.
    /// </summary>
    [Required, StringLength(255), CanonicalName]
    public string Value { get; } = "";

    /// <inheritdoc />
    [Range(60, 7200)]
    public int TimeToLive { get; } = 1799;

    int IHostRecord.Preference => 0;

    IHostRecord IHostRecord.Create(string name, string value, int timeToLive, int preference)
        => new HostNameserverRecord(name, value, timeToLive);
}
