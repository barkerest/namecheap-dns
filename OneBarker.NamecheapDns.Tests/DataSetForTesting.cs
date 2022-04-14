using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OneBarker.NamecheapDns.Enums;
using OneBarker.NamecheapDns.Models;

namespace OneBarker.NamecheapDns.Tests;

public class DataSetForTesting
{
    public static readonly int CountOf_WWW_A;
    public static readonly int CountOf_WWW_AAAA;
    public static readonly int CountOf_A;
    public static readonly int CountOf_AAAA;
    public static readonly int CountOf_CNAME;
    public static readonly int CountOf_MX;

    public static readonly string[] InitialNames;

    public const string IpForMail1 = "1.2.3.3";
    public const string AnIpForWww = "1.2.3.4";

    public static ICollection<IHostRecord> Create()
        =>  new List<IHostRecord>()
        {
            new HostIPv4Record("www", AnIpForWww, 3600),
            new HostIPv4Record("www", "4.3.2.1", 3600),
            new HostIPv6Record("www", "0:1:2:3::4", 7200),
            new HostIPv4Record("mail1", IpForMail1, 3600),
            new HostIPv4Record("mail2", "4.3.2.2", 3600),
            new HostCNameRecord("www-2", "www.example.com.", 3600),
            new HostMailRecord("@", "mail1.example.com.", 7200, 10),
            new HostMailRecord("@", "mail2.example.com.", 7200, 20),
        };

    static DataSetForTesting()
    {
        var data = Create();
        CountOf_WWW_A    = data.Count(x => x.Name.Equals("www", StringComparison.OrdinalIgnoreCase) && x.Type == RecordType.A);
        CountOf_WWW_AAAA = data.Count(x => x.Name.Equals("www", StringComparison.OrdinalIgnoreCase) & x.Type == RecordType.AAAA);
        CountOf_A        = data.Count(x => x.Type == RecordType.A);
        CountOf_AAAA     = data.Count(x => x.Type == RecordType.AAAA);
        CountOf_CNAME    = data.Count(x => x.Type == RecordType.CNAME);
        CountOf_MX       = data.Count(x => x.Type == RecordType.MX);
        InitialNames     = data.Select(x => x.Name.ToLower()).Distinct().OrderBy(x => x).ToArray();
    }

}
