using System.Net;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () =>
    {
        var on = DateTime.UtcNow;
        var hostname = Dns.GetHostName();
        var ipList = new List<string>();
        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType is NetworkInterfaceType.Ethernet or NetworkInterfaceType.Wireless80211)
            {
                foreach (var address in ni.GetIPProperties().UnicastAddresses)
                {
                    var ip = address.Address.ToString();
                    if (!string.IsNullOrWhiteSpace(ip))
                    {
                        ipList.Add(ip);
                    }
                }
            }
        }

        return new { on, responseFrom = hostname, ip = ipList };
    }
).DisableAntiforgery();

app.Run();