using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapDns.Attributes;
using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// [ALIAS] An alias record.
/// </summary>
public class HostAliasRecord : IHostRecord
{
    public HostAliasRecord()
    {
    }

    public HostAliasRecord(string name, string value, int timeToLive)
    {
        Name       = name;
        Value      = value;
        TimeToLive = timeToLive;
    }

    /// <inheritdoc />
    public RecordType Type => RecordType.ALIAS;

    /// <inheritdoc />
    [Required, StringLength(63)]
    public string Name { get; } = "";

    /// <summary>
    /// A host name that this alias references.
    /// </summary>
    [Required, StringLength(255), CanonicalName]
    public string Value { get; } = "";

    /// <inheritdoc />
    [Range(60, 7200)]
    public int TimeToLive { get; } = 300;


    int IHostRecord.Preference => 0;

    IHostRecord IHostRecord.Create(string name, string value, int timeToLive, int preference)
        => new HostAliasRecord(name, value, timeToLive);

    
}
