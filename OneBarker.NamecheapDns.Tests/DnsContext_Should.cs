using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Logging;

namespace OneBarker.NamecheapDns.Tests;

public class DnsContext_Should
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger        _logger;
    
    public DnsContext_Should(ITestOutputHelper outputHelper)
    {
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(outputHelper, Config.BasicLoggingFilter));
        _logger = _loggerFactory.CreateLogger<DnsContext_Should>();
    }

    private IDnsContext GetContext(bool checkForDomains)
    {
        var cfg = Config.ApiConfig;
        if (string.IsNullOrWhiteSpace(cfg.ApiUser) ||
            string.IsNullOrWhiteSpace(cfg.ApiKey) ||
            string.IsNullOrWhiteSpace(cfg.ClientIP))
        {
            throw new ApplicationException("The API configuration is not set correctly.");
        }

        var ret = new DnsContext(cfg, _loggerFactory.CreateLogger<DnsContext>());
        if (checkForDomains)
        {
            if (ret.RegisteredDomains.Count < 1)
            {
                throw new ApplicationException("The account has no domains to test.");
            }
        }

        return ret;
    }

    [Fact]
    public void LoadTopLevelDomains()
    {
        var ctx = GetContext(false);
        Assert.NotEmpty(ctx.TopLevelDomains);
        _logger.LogInformation(string.Join(", ", ctx.TopLevelDomains));
    }

    [Fact]
    public void LoadRegisteredDomains()
    {
        var ctx    = GetContext(true);
        _logger.LogInformation(string.Join(", ", ctx.RegisteredDomains.Select(x => x.DomainName)));
    }

    [Fact]
    public void WorkWithHosts()
    {
        var ctx = GetContext(true);
        var dom = ctx.RegisteredDomains.First();
        _logger.LogInformation($"Working with {dom.DomainName}...");

        if (dom.HostRecords.Any())
        {
            _logger.LogInformation($"The domain currently has {dom.HostRecords.Keys.Count} hosts.");
        }
        else
        {
            _logger.LogInformation($"The domain currently has no hosts defined.");
        }

        var hasWww = dom.HostRecords.ContainsKey("www");
        if (hasWww)
        {
            _logger.LogInformation("Removing www and adding mmm.");
            dom.HostRecords.Remove("www");
            dom.HostRecords["mmm"].IPv4Addresses.SetValue("1.1.1.1\n2.2.2.2\n3.3.3.3");
        }
        else if (dom.HostRecords.ContainsKey("mmm"))
        {
            _logger.LogInformation("Removing mmm and adding www.");
            dom.HostRecords.Remove("mmm");
            dom.HostRecords["www"].IPv4Addresses.SetValue("4.4.4.4\n5.5.5.5\n6.6.6.6");
        }
        else
        {
            _logger.LogInformation("Adding www.");
            dom.HostRecords["www"].IPv4Addresses.SetValue("4.4.4.4\n5.5.5.5\n6.6.6.6");
        }

        var success = dom.Save();
        if (!success)
        {
            _logger.LogWarning(string.Join("\n", dom.SaveErrors));
            Assert.True(success);
        }
        
        dom.Reload();

        Assert.Equal(!hasWww, dom.HostRecords.ContainsKey("www"));
        Assert.Equal(hasWww, dom.HostRecords.ContainsKey("mmm"));

        _logger.LogInformation($"The domain now has {dom.HostRecords.Keys.Count} hosts.");
        
        Assert.Equal(3, dom.HostRecords[hasWww ? "mmm" : "www"].IPv4Addresses.Count);
    }
}
