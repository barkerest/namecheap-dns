using System.Diagnostics;
using System.Xml;
using Microsoft.Extensions.Logging;
using OneBarker.NamecheapDns.Exceptions;
using OneBarker.NamecheapDns.Models;
using OneBarker.NamecheapDns.Utility;

namespace OneBarker.NamecheapDns;

/// <summary>
/// A context for managing Namecheap DNS.
/// </summary>
public class DnsContext : IDnsContext
{
    
    private readonly IDnsContextConfig       _config;
    private readonly ILogger                 _logger;
    private readonly List<IRegisteredDomain> _domains = new();
    private          bool                    _loaded  = false;
    private          string[]?               _topLevel;

    /// <summary>
    /// Create a new DNS context.
    /// </summary>
    /// <param name="config">The configuration for this context.</param>
    /// <param name="logger">The logger for this context.</param>
    public DnsContext(IDnsContextConfig config, ILogger<DnsContext> logger)
    {
        // clone the config to prevent changes during runtime.
        _config  = new DnsContextConfig(config.ApiHost, config.ApiUser, config.ApiKey, config.ClientIP, config.UserName);
        _logger  = logger;
        
    }

    #region Rate Limiting

    // https://www.namecheap.com/support/knowledgebase/article.aspx/9739/63/api-faq/#z
    private const int CallsPerMinute = 50;

    private const int CallsPerHour = 700;

    private const int CallsPerDay = 8000;

    private static readonly Dictionary<string, List<(DateTime timestamp, string command)>> CallLog = new();

    private async Task WaitForCallSlot(string command, int milliseconds)
    {
        using (var cts = new CancellationTokenSource())
        {
            cts.CancelAfter(milliseconds);
            await Task.Run(() => WaitForCallSlot(command, cts.Token));
        }
    }
    
    private void WaitForCallSlot(string command, CancellationToken token)
    {
        var key = $"{_config.ApiUser}:{_config.ApiKey}";
        lock (CallLog)
        {
            if (!CallLog.ContainsKey(key)) CallLog[key] = new List<(DateTime timestamp, string command)>();

            // haven't hit the shortest limit yet, so, we can safely allow the call to continue.
            if (CallLog[key].Count < CallsPerMinute &&
                CallLog[key]
                    .DefaultIfEmpty((timestamp: DateTime.Now.AddDays(-1), command: ""))
                    .LastOrDefault()
                    .timestamp < DateTime.Now)
            {
                CallLog[key].Add((DateTime.Now.AddMilliseconds(100), command));
                return;
            }
        }

        while (true)
        {
            lock (CallLog)
            {
                var list   = CallLog[key];
                var cutoff = DateTime.Now.AddDays(-1);

                // remove all calls logged over 24 hours ago.
                list.RemoveAll(x => x.timestamp < cutoff);

                // we have at least one free slot in a 24 hour window.
                if (list.Count < CallsPerDay)
                {
                    cutoff = DateTime.Now.AddHours(-1);

                    // we have at least one free slot in a 1 hour window.
                    if (list.Count(x => x.timestamp >= cutoff) < CallsPerHour)
                    {
                        cutoff = DateTime.Now.AddMinutes(-1);

                        // we have at least one free slot in a 1 minute window.
                        if (list.Count(x => x.timestamp >= cutoff) < CallsPerMinute)
                        {
                            list.Add((DateTime.Now.AddMilliseconds(100), command));
                            return;
                        }
                    }
                }
            }

            token.ThrowIfCancellationRequested();

            Thread.Sleep(10);
        }
    }

    #endregion

    /// <summary>
    /// The client used to execute commands.
    /// </summary>
    private static readonly HttpClient Client = new();

    private async Task<XmlElement> ExecuteApiCommandAsync(string command, params KeyValuePair<string, string>[] parameters)
    {
        var content = new FormUrlEncodedContent(
            new[]
                {
                    new KeyValuePair<string, string>("ApiUser", _config.ApiUser),
                    new KeyValuePair<string, string>("ApiKey", _config.ApiKey),
                    new KeyValuePair<string, string>("Command", command),
                    new KeyValuePair<string, string>("UserName", _config.UserName),
                    new KeyValuePair<string, string>("ClientIP", _config.ClientIP),
                }
                .Concat(parameters)
        );
        
        var apiUri = $"https://{_config.ApiHost}/xml.response";

        // wait up to 30 seconds for a call slot to be available.
        await WaitForCallSlot(command, 30000);
        
        var sw = new Stopwatch();
        
        _logger.LogDebug($"Sending request via POST\n{apiUri}?Command={command}");
        sw.Start();
        var response = await Client.PostAsync(apiUri, content);
        sw.Stop();
        _logger.LogDebug($"HTTP request completed in {sw.ElapsedMilliseconds:#,##0}ms.");
        
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException("Command execution failed.", null, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();

        _logger.LogDebug("Parsing XML response...");
        var xml = new XmlDocument();
        xml.LoadXml(responseContent);

        if (xml.DocumentElement is not { } root)
            throw new XmlException("No XML content generated.");

        root.RequireName("ApiResponse");

        if (!root.GetAttribute("Status").Equals("OK", StringComparison.OrdinalIgnoreCase))
        {
            var errElement = root.GetRequiredChild("Errors");
            var errs       = errElement.ChildNodes.OfType<XmlElement>().Where(x => x.Name == "Error").ToArray();
            if (errs.Length == 0)
                throw new MissingElementException("Errors", "Error");

            throw new ApiErrorException(errs.Select(x => (x.GetAttributeAsInt32("Number"), x.GetContent())));
        }

        return root.GetRequiredChild("CommandResponse");
    }

    XmlElement IDnsContext.ExecuteApiCommand(string command, params KeyValuePair<string, string>[] parameters)
        => ExecuteApiCommandAsync(command, parameters).Result;

    /// <inheritdoc />
    public void Reload()
    {
        var page    = 1;
        var maxPage = 1;
        var data = new Dictionary<string, string>()
        {
            { "ListType", "ALL" },
            { "PageSize", "100" },
            { "SortBy", "NAME" },
        };
        
        _domains.Clear();

        while (page <= maxPage)
        {
            data["Page"] = page.ToString();
            _logger.LogDebug($"Loading page {page} of domains.");
            var response = ((IDnsContext)this).ExecuteApiCommand(ApiCommand.GetDomainList, data.ToArray());
            if (response.GetChild(ApiCommand.GetDomainListResult) is not { } list) return;

            var paging     = response.GetRequiredChild("Paging");
            var totalItems = paging.GetRequiredChildContentAsInt32("TotalItems");
            var perPage    = paging.GetRequiredChildContentAsInt32("PageSize");
            
            // stay consistent with the API.
            data["PageSize"] = perPage.ToString();
            page             = paging.GetRequiredChildContentAsInt32("CurrentPage");

            maxPage          = (totalItems + perPage - 1) / perPage;
            page++;

            foreach (var listItem in list.ChildNodes.OfType<XmlElement>().Where(x => x.Name.Equals("Domain", StringComparison.OrdinalIgnoreCase)))
            {
                _domains.Add(new RegisteredDomain(this, listItem));
            }
        }

        _logger.LogInformation($"Loaded {_domains.Count} domains.");
    }

    /// <inheritdoc />
    public IReadOnlyList<IRegisteredDomain> RegisteredDomains
    {
        get
        {
            if (!_loaded) Reload();
            return _domains;
        }
    }

    private string[] LoadTopLevelDomains()
    {
        var response = ((IDnsContext)this).ExecuteApiCommand(ApiCommand.GetTopLevelDomainList);
        
        if (response.GetChild(ApiCommand.GetTopLevelDomainListResult) is { } result)
        {
            return result.ChildNodes
                         .OfType<XmlElement>()
                         .Where(x => x.Name.Equals("Tld", StringComparison.OrdinalIgnoreCase))
                         .Select(x => "." + x.GetRequiredAttribute("Name"))
                         .ToArray();
        }

        _logger.LogError("Failed to retrieve list of TLDs from Namecheap, falling back on original values.");
        
        return new[] { ".com", ".net", ".org", ".edu", ".gov", ".mil" };
    }

    /// <inheritdoc />
    public IReadOnlyList<string> TopLevelDomains
        => _topLevel ??= LoadTopLevelDomains();
}
