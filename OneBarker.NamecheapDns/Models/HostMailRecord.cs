using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapDns.Attributes;
using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// [MX] A canonical name record.
/// </summary>
public class HostMailRecord : IHostRecord
{
    public HostMailRecord()
    {
    }

    public HostMailRecord(string name, string value, int timeToLive, int preference)
    {
        Name       = name;
        Value      = value;
        TimeToLive = timeToLive;
        Preference = preference;
    }

    /// <inheritdoc />
    public RecordType Type => RecordType.MX;

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

    /// <inheritdoc />
    public int Preference { get; } = 5;

    IHostRecord IHostRecord.Create(string name, string value, int timeToLive, int preference)
        => new HostMailRecord(name, value, timeToLive, preference);
}
