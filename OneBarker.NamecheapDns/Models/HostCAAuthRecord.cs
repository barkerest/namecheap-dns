using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// A certificate authority authorization record.
/// </summary>
public class HostCAAuthRecord : IHostRecord
{
    private static readonly Regex ValuePattern = new(@"^(?<FLAG>\d+)\s+(?<TAG>\w+)\s+""?(?<CA>[^""]+)""?$");

    public HostCAAuthRecord()
    {
    }

    public HostCAAuthRecord(string name, CertificateAuthorityAuthTag tag, string certificateAuthority, int timeToLive, byte flag = 0)
    {
        Name                 = name;
        Tag                  = tag;
        CertificateAuthority = certificateAuthority;
        TimeToLive           = timeToLive;
        Flag                 = flag;
    }

    public HostCAAuthRecord(string name, string value, int timeToLive)
    {
        var valueMatch = ValuePattern.Match(value);

        var flag = valueMatch.Success && byte.TryParse(valueMatch.Groups["FLAG"].Value, out var f) ? f : (byte)0;
        var tag  = valueMatch.Success && Enum.TryParse<CertificateAuthorityAuthTag>(valueMatch.Groups["TAG"].Value, false, out var t) ? t : CertificateAuthorityAuthTag.Issue;
        var ca   = valueMatch.Success ? valueMatch.Groups["CA"].Value : "";

        Name                 = name;
        Tag                  = tag;
        CertificateAuthority = ca;
        TimeToLive           = timeToLive;
        Flag                 = flag;
    }

    /// <inheritdoc />
    public RecordType Type => RecordType.CAA;

    /// <inheritdoc />
    public string Name { get; } = "";

    string IHostRecord.Value => $"{Flag} {Tag.ToString().ToLower()} \"{CertificateAuthority}\"";

    /// <summary>
    /// The flag for this record (usually 0).
    /// </summary>
    public byte Flag { get; } = 0;

    /// <summary>
    /// The tag for this record.
    /// </summary>
    public CertificateAuthorityAuthTag Tag { get; } = CertificateAuthorityAuthTag.Issue;

    /// <summary>
    /// The certificate authority address.
    /// </summary>
    public string CertificateAuthority { get; } = "";

    /// <inheritdoc />
    public int TimeToLive { get; } = 1799;

    int IHostRecord.Preference => 0;


    IHostRecord IHostRecord.Create(string name, string value, int timeToLive, int preference)
    {
        return new HostCAAuthRecord(name, value, timeToLive);
    }
}
