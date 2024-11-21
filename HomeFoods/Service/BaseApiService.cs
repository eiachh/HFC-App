using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HomeFoods.Service
{
    public class BaseApiService
    {
        public static string Domain { get; set; } = "eiachh.duckdns.org";
        public static string DefaultPublicIp { get; set; } = "84.3.93.248";
        public static string Port { get; set; } = "30021";
        public static string ApiServerUrl { get; private set; } = "http://192.168.0.69:1323";

        private static bool initDone = false;
        public BaseApiService()
        {
            Init();
        }
        public void Init()
        {
            if(initDone)
                return;

            var defaultUrl = "http://" + DefaultPublicIp + ":" + Port;
            var isReachable = Task.Run(() => IsServerReachable(defaultUrl)).GetAwaiter().GetResult();
            if (isReachable)
            {
                ApiServerUrl = defaultUrl;
                initDone = true;
                return;
            }
            else
            {
                var ipv4 = "";
                IPAddress[] addresses = Dns.GetHostAddresses(Domain);
                foreach (var ipadress in addresses)
                {
                    if (ipadress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipv4 = ipadress.ToString();
                    }
                }
                DefaultPublicIp = ipv4;
                if (ipv4 != "")
                {
                    ApiServerUrl = "http://" + ipv4 + ":" + Port;
                }


            }
            var isResolvedReachable = Task.Run(() => IsServerReachable(ApiServerUrl)).GetAwaiter().GetResult();
            if (!isResolvedReachable)
                throw new Exception("Could not find any valid connection to the HFC server.");
            initDone = true;
        }
        public async Task<bool> IsServerReachable(string url)
        {
            url += "/healthy";
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
