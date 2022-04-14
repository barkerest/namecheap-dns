using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using OneBarker.NamecheapDns.Models;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapDns.Tests;

public class HostRecordSets_Should
{
    private readonly ITestOutputHelper _output;

    public HostRecordSets_Should(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }
    
    
    [Fact]
    public void WorkWithSharedLists()
    {
        var list = DataSetForTesting.Create();
        var rec1  = new HostRecordSet(list, "www");
        var rec2 = new HostRecordSet(list, "www");

        Assert.Equal(DataSetForTesting.CountOf_WWW_A, rec1.IPv4Addresses.Count);
        Assert.Equal(DataSetForTesting.CountOf_WWW_AAAA, rec1.IPv6Addresses.Count);
        Assert.Equal(rec1.IPv4Addresses.Count, rec2.IPv4Addresses.Count);
        Assert.Equal(rec1.IPv6Addresses.Count, rec2.IPv6Addresses.Count);
        rec1.IPv4Addresses.Clear();
        Assert.Equal(0, rec1.IPv4Addresses.Count);
        Assert.Equal(0, rec2.IPv4Addresses.Count);
        Assert.Equal(DataSetForTesting.CountOf_WWW_AAAA, rec1.IPv6Addresses.Count);
        Assert.Equal(DataSetForTesting.CountOf_WWW_AAAA, rec2.IPv6Addresses.Count);
        rec2.IPv4Addresses.Add("5.6.7.8");
        Assert.Equal(1, rec1.IPv4Addresses.Count);
        Assert.Equal(1, rec2.IPv4Addresses.Count);
    }

    [Fact]
    public void WorkWithIndependentLists()
    {
        var www  = new HostRecordSet("www") { IPv4Addresses = { "1.2.3.4" } };
        var www2 = new HostRecordSet("www") { IPv4Addresses = { "4.3.2.1", "5.4.3.2" } };

        Assert.Equal(1, www.IPv4Addresses.Count);
        Assert.Equal(2, www2.IPv4Addresses.Count);
        www.IPv4Addresses.Clear();
        Assert.Equal(0, www.IPv4Addresses.Count);
        Assert.Equal(2, www2.IPv4Addresses.Count);
    }

    [Fact]
    public void ProvideCertainValidations()
    {
        var list   = DataSetForTesting.Create();
        var www    = new HostRecordSet(list, "www");
        var val    = (IValidatableObject)www;
        var errors = val.Validate(new ValidationContext(www)).ToArray();
        
        // the test list should be valid.
        Assert.Empty(errors);

        www.CanonicalName.SetValue("something.example.com.");
        Assert.Equal("something.example.com.", www.CanonicalName.Value);
        
        errors = val.Validate(new ValidationContext(www)).ToArray();
        Assert.NotEmpty(errors);
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(IHostRecordSet.CanonicalName)));
        www.CanonicalName.ClearValue();
        
        errors = val.Validate(new ValidationContext(www)).ToArray();
        Assert.Empty(errors);

        // setup all the error prone items.
        www.CanonicalName.SetValue("something.example.com.");
        www.MaskedRedirect.SetValue("https://www2.example.com");
        www.UnmaskedRedirect.SetValue("https://www2.example.com");
        www.PermanentRedirect.SetValue("https://www2.example.com");
        www.MailServers.Add("mail3.example.com.");
        www.MailAddress.SetValue("4.3.5.6");
        
        errors = val.Validate(new ValidationContext(www)).ToArray();
        Assert.NotEmpty(errors);
        
        // dump the errors.
        _output.WriteLine(string.Join("\n", errors.Select(x => string.Join(", ", x.MemberNames) + ' ' + (x.ErrorMessage ?? "appears invalid"))));
        
        // verify each one of the above is in the error list.
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(IHostRecordSet.CanonicalName)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(IHostRecordSet.MaskedRedirect)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(IHostRecordSet.UnmaskedRedirect)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(IHostRecordSet.PermanentRedirect)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(IHostRecordSet.MailAddress)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(IHostRecordSet.MailServers)));
    }
}

