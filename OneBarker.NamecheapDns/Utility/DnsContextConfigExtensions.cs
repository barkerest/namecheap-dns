using System.Net;
using System.Net.Sockets;
using OneBarker.NamecheapDns.Models;

namespace OneBarker.NamecheapDns.Utility;

public static class DnsContextConfigExtensions
{
    private static readonly HttpClient WebClient = new();

    private static readonly string[] EchoServiceUrls = 
    {
        "http://checkip.amazonaws.com/",
        "http://icanhazip.com/",
    };

    private static string   _previousPublicIp     = "";
    private static DateTime _previousPublicIpTime = DateTime.MinValue;


    /// <summary>
    /// Finds the public IP address of this machine.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="forceRefresh">Force the system to refresh the IP even if the previous lookup is less than 10 minutes old.</param>
    /// <returns></returns>
    public static string FindPublicIP(this IDnsContextConfig _, bool forceRefresh = false)
    {
        if (!forceRefresh &&
            _previousPublicIpTime.AddMinutes(10) >= DateTime.Now)
        {
            return _previousPublicIp;
        }
        
        foreach (var ipEchoUrl in EchoServiceUrls)
        {
            try
            {
                var pubIp = WebClient.GetStringAsync(ipEchoUrl).Result.Trim();
                if (IPAddress.TryParse(pubIp, out var ip) && ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    _previousPublicIp = ip.ToString();
                    _previousPublicIpTime = DateTime.Now;
                    return _previousPublicIp;
                }
            }
            catch (Exception e) when (e is HttpRequestException or FormatException)
            {
                // continue
            }
        }

        _previousPublicIp = IPAddress.None.ToString();
        _previousPublicIpTime = DateTime.Now;
        
        return _previousPublicIp;
    }
    
    
}
