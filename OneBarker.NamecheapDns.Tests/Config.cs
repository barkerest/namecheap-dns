using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneBarker.NamecheapDns.Models;

namespace OneBarker.NamecheapDns.Tests;

public static class Config
{
    public static IConfiguration    Configuration { get; }
    public static IDnsContextConfig ApiConfig     { get; }

    static Config()
    {
        var cfgBuilder = new ConfigurationBuilder();
        var files = new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            }.Where(x => !string.IsNullOrWhiteSpace(x))
             .Select(x => x.Replace('\\', '/').TrimEnd('/') + "/OneBarker/test-config.json")
             .Distinct()
             .Append(Environment.CurrentDirectory.Replace('\\', '/').TrimEnd('/') + "/test-config.json")
             .ToArray();

        foreach (var file in files)
        {
            cfgBuilder.AddJsonFile(file, true, false);
        }

        cfgBuilder.AddEnvironmentVariables("ONEB_");
        cfgBuilder.AddCommandLine(
            Environment.GetCommandLineArgs(),
            new Dictionary<string, string>()
            {
                { "--api-user", "NamecheapApi:ApiUser" },
                { "--api-key", "NamecheapApi:ApiKey" },
                { "--test-domain", "NamecheapApi:ExplicitTestDomain" },
            }
        );

        Configuration = cfgBuilder.Build();
        
        ApiConfig = new DnsContextConfig(
            DnsContextConfig.SandboxHost,
            Configuration["NamecheapApi:ApiUser"],
            Configuration["NamecheapApi:ApiKey"]
        );
    }

    public static bool BasicLoggingFilter(string category, LogLevel level)
    {
        if (level >= LogLevel.Error) return true;

        category = category.ToLower();
        if (category.StartsWith("microsoft.") ||
            category.StartsWith("system.")) return level >= LogLevel.Warning;

        return true;
    }
}
