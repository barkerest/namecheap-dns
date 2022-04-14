using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapDns.Attributes;
using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// [MXE] An IPv4 address record.
/// </summary>
public class HostMailEasyRecord : IHostRecord
{
    public HostMailEasyRecord()
    {
    }

    public HostMailEasyRecord(string name, string value, int timeToLive)
    {
        Name       = name;
        Value      = value;
        TimeToLive = timeToLive;
    }

    /// <inheritdoc />
    public RecordType Type => RecordType.MXE;

    /// <inheritdoc />
    [Required, StringLength(63)]
    public string Name { get; } = "";

    /// <summary>
    /// The IP address for this record.
    /// </summary>
    [Required, StringLength(255), IPv4Address]
    public string Value { get; } = "";

    /// <inheritdoc />
    [Range(60, 7200)]
    public int TimeToLive { get; } = 1799;

    int IHostRecord.Preference => 0;

    IHostRecord IHostRecord.Create(string name, string value, int timeToLive, int preference)
        => new HostMailEasyRecord(name, value, timeToLive);
}
