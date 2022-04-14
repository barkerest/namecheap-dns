using System.Collections.Generic;
using System.Data;
using System.Linq;
using OneBarker.NamecheapDns.Enums;
using OneBarker.NamecheapDns.Models;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapDns.Tests;

public class HostRecordCollections_Should
{
    private readonly ITestOutputHelper _output;

    public HostRecordCollections_Should(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    [Fact]
    public void WorkWithSharedSource()
    {
        var list  = DataSetForTesting.Create();
        var cntA  = list.Count(x => x.Type == RecordType.A);
        var cntA4 = list.Count(x => x.Type == RecordType.AAAA);
        var cntC  = list.Count(x => x.Type == RecordType.CNAME);
        var cntM  = list.Count(x => x.Type == RecordType.MX);

        var rec1 = new HostRecordCollection<HostIPv4Record>(list, "www");
        var rec2 = new HostRecordCollection<HostIPv4Record>(list, "www");

        Assert.Equal(DataSetForTesting.CountOf_WWW_A, rec1.Count);
        rec1.ClearValue();
        Assert.Equal(0, rec1.Count);
        Assert.Equal(rec2.Count, rec1.Count);
        Assert.Equal(cntA - DataSetForTesting.CountOf_WWW_A, list.Count(x => x.Type == RecordType.A));
        Assert.Equal(cntA4, list.Count(x => x.Type == RecordType.AAAA));
        Assert.Equal(cntC, list.Count(x => x.Type == RecordType.CNAME));
        Assert.Equal(cntM, list.Count(x => x.Type == RecordType.MX));
        rec1.Add("4.3.4.3");
        rec1.Add("3.4.3.4");
        rec2.Add("4.4.3.3");
        rec2.Add("3.3.4.4");
        Assert.Equal(4, rec1.Count);
        Assert.Equal(rec2.Count, rec1.Count);
        Assert.Equal(cntA - DataSetForTesting.CountOf_WWW_A + 4, list.Count(x => x.Type == RecordType.A));
        Assert.Equal(cntA4, list.Count(x => x.Type == RecordType.AAAA));
        Assert.Equal(cntC, list.Count(x => x.Type == RecordType.CNAME));
        Assert.Equal(cntM, list.Count(x => x.Type == RecordType.MX));

        var rec3 = new HostRecordCollection<HostIPv4Record>(list, "mail1");
        Assert.Equal(1, rec3.Count);
        // test out create.
        // in this scenario, it should alter and link to the shared list.
        rec2 = (HostRecordCollection<HostIPv4Record>)((IHostRecord)rec1).Create("mail1", "1.2.3.4\n1.2.3.5\n1.2.3.6", 3600, 10);
        Assert.Equal(3, rec2.Count);
        Assert.Equal(3, rec3.Count);

        // we can also overwrite the existing list.
        rec2 = (HostRecordCollection<HostIPv4Record>)((IHostRecord)rec1).Create("www", "1.2.3.4\n1.2.3.5\n1.2.3.6", 3600, 10);
        Assert.Equal(3, rec1.Count);
        Assert.Equal(3, rec2.Count);
    }

    [Fact]
    public void WorkWithIndependentLists()
    {
        var www  = new HostRecordCollection<HostIPv4Record>("www", new[] { "1.2.3.4", "4.3.2.1" });
        var www2 = (HostRecordCollection<HostIPv4Record>)((IHostRecord)www).Create("www", "1.2.3.4\n1.2.3.5\n1.2.3.6", 3600, 10);

        Assert.Equal(2, www.Count);
        Assert.Contains("1.2.3.4", www);
        Assert.Equal(3, www2.Count);
        Assert.Contains("1.2.3.4", www2);
        Assert.DoesNotContain("1.2.3.5", www);
        Assert.Contains("1.2.3.5", www2);
    }

    [Fact]
    public void UpdateTimeToLive()
    {
        var www = new HostRecordCollection<HostIPv4Record>(DataSetForTesting.Create(), "www");
        Assert.Equal(DataSetForTesting.CountOf_WWW_A, www.Count);
        Assert.Equal(3600, www.TimeToLive);
        Assert.Equal(3600, ((IHostRecordAccessor)www).ToHostRecords().First().TimeToLive);
        www.ChangeTimeToLive(4000);
        Assert.Equal(DataSetForTesting.CountOf_WWW_A, www.Count);
        Assert.Equal(4000, www.TimeToLive);
        Assert.Equal(4000, ((IHostRecordAccessor)www).ToHostRecords().First().TimeToLive);
    }
}
