using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapDns.Attributes;
using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// [TXT] A text record.
/// </summary>
public class HostTextRecord : IHostRecord
{
    public HostTextRecord()
    {
    }

    public HostTextRecord(string name, string value, int timeToLive)
    {
        Name       = name;
        Value      = value;
        TimeToLive = timeToLive;
    }

    /// <inheritdoc />
    public RecordType Type => RecordType.TXT;

    /// <inheritdoc />
    [Required, StringLength(63)]
    public string Name { get; } = "";

    /// <inheritdoc />
    [Required, StringLength(255)]
    public string Value { get; } = "";

    /// <inheritdoc />
    [Range(60, 7200)]
    public int TimeToLive { get; } = 1799;


    int IHostRecord.Preference => 0;

    IHostRecord IHostRecord.Create(string name, string value, int timeToLive, int preference)
        => new HostTextRecord(name, value, timeToLive);
}
