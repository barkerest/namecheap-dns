using System.Data;
using System.Linq;
using OneBarker.NamecheapDns.Models;
using OneBarker.NamecheapDns.Utility;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapDns.Tests;

public class HostRecordDictionary_Should
{
    private readonly ITestOutputHelper _output;

    public HostRecordDictionary_Should(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    [Fact]
    public void WorkWithSharedData()
    {
        var list  = DataSetForTesting.Create();
        var data  = new HostRecordDictionary(list);
        var data2 = new HostRecordDictionary(list);

        // test one, all the predefined keys exist.
        Assert.True(DataSetForTesting.InitialNames.SequenceEqual(data.Keys));
        Assert.True(DataSetForTesting.InitialNames.SequenceEqual(data2.Keys));
        
        // test two, counts for the WWW data set.
        Assert.True(data.ContainsKey("www"));
        Assert.True(data.ContainsKey("WWW"));   // also include case-insensitive test just for good measure.
        Assert.Equal(DataSetForTesting.CountOf_WWW_A, data["www"].IPv4Addresses.Count);
        Assert.Equal(data["www"].IPv4Addresses.Count, data["WWW"].IPv4Addresses.Count);
        // and between instances.
        Assert.Equal(data["www"].IPv4Addresses.Count, data2["www"].IPv4Addresses.Count);

        // test three, both contain the www-2 record.
        Assert.True(data.ContainsKey("www-2"));
        Assert.True(data2.ContainsKey("www-2"));
        Assert.False(string.IsNullOrWhiteSpace(data["www-2"].CanonicalName.Value));
        Assert.Equal(data["www-2"].CanonicalName.Value, data2["www-2"].CanonicalName.Value);
        
        // test four, remove the www-2 record.
        data.Remove("www-2");
        Assert.False(data.ContainsKey("www-2"));
        Assert.False(data2.ContainsKey("www-2"));
        
        // we should be able to access those that don't exist still.
        Assert.True(string.IsNullOrWhiteSpace(data["www-2"].CanonicalName.Value));
        
        // but they still shouldn't be included unless we set a new value.
        Assert.False(data.ContainsKey("www-2"));
        
        // test five, add the www-2 record back in.
        data["www-2"].CanonicalName.SetValue("hello.world.");
        Assert.True(data.ContainsKey("www-2"));
        Assert.True(data2.ContainsKey("www-2"));
        Assert.Equal("hello.world.", data["www-2"].CanonicalName.Value);
        Assert.Equal("hello.world.", data2["www-2"].CanonicalName.Value);
    }

    [Fact]
    public void WorkWithIndependentData()
    {
        var data  = new HostRecordDictionary();
        var data2 = new HostRecordDictionary();

        data["www"].IPv4Addresses.Add("1.2.3.4");
        data2["www"].IPv4Addresses.SetValue("4.3.2.1\n5.4.3.2");

        Assert.True(data.ContainsKey("www"));
        Assert.True(data2.ContainsKey("www"));

        Assert.Equal(1, data["www"].IPv4Addresses.Count);
        Assert.Equal(2, data2["www"].IPv4Addresses.Count);
    }

    [Fact]
    public void ValidateData()
    {
        var      list = DataSetForTesting.Create();
        var      data = new HostRecordDictionary(list);
        string[] errors;

        var valid = data.IsValid(out errors);

        if (!valid)
        {
            _output.WriteLine("Test 0 Errors:\n" + string.Join('\n', errors));
            Assert.True(valid);
        }

        // test 1: cause an error in a data set.
        data["www"].CanonicalName.SetValue("some.place.else.");
        valid = data.IsValid(out errors);
        Assert.False(valid);
        _output.WriteLine("Test 1 Errors:\n" + string.Join('\n', errors));
        Assert.Contains(errors, x => x.StartsWith("Host: www, ") && x.Contains(nameof(IHostRecordSet.CanonicalName)));
        
        data["www"].CanonicalName.ClearValue();
        Assert.True(data.IsValid(out _));

        // test 2: cause an error in an individual record.
        data["www"].IPv4Addresses.Add("a.b.c.d");
        valid = data.IsValid(out errors);
        Assert.False(valid);
        _output.WriteLine("Test 2 Errors:\n" + string.Join('\n', errors));
        Assert.Contains(errors, x => x.StartsWith("Host: www, Type: A, ") && x.Contains("valid IPv4"));
    }

}
