using System.Collections.Generic;
using System.Linq;
using OneBarker.NamecheapDns.Enums;
using OneBarker.NamecheapDns.Models;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapDns.Tests;

public class HostRecordAccessors_Should
{
    private readonly ITestOutputHelper _output;

    public HostRecordAccessors_Should(ITestOutputHelper outputHelper)
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

        var rec1 = new HostRecordAccessor<HostIPv4Record>(list, "mail1");
        var rec2 = new HostRecordAccessor<HostIPv4Record>(list, "mail1");

        Assert.Equal(rec1.Value, rec2.Value);
        Assert.Equal(DataSetForTesting.IpForMail1, rec1.Value);
        rec1.ClearValue();
        Assert.Equal("", rec1.Value);
        Assert.Equal(rec2.Value, rec1.Value);
        Assert.Equal(cntA - 1, list.Count(x => x.Type == RecordType.A));
        Assert.Equal(cntA4, list.Count(x => x.Type == RecordType.AAAA));
        Assert.Equal(cntC, list.Count(x => x.Type == RecordType.CNAME));
        Assert.Equal(cntM, list.Count(x => x.Type == RecordType.MX));
        rec1.SetValue("3.3.2.2");
        Assert.Equal("3.3.2.2", rec1.Value);
        Assert.Equal(rec2.Value, rec1.Value);

        rec1.SetValue("3.3.3.3");
        Assert.Equal("3.3.3.3", rec1.Value);
        Assert.Equal(rec2.Value, rec1.Value);
    }

    [Fact]
    public void WorkWithIndependentLists()
    {
        var www  = new HostRecordAccessor<HostIPv4Record>("www", "1.2.3.4");
        var www2 = (HostRecordAccessor<HostIPv4Record>)((IHostRecord)www).Create("www", "4.3.2.1", www.TimeToLive, 0);

        Assert.Equal("1.2.3.4", www.Value);
        Assert.Equal("4.3.2.1", www2.Value);
    }
}
