using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer
{
    public class SocketConnection
    {
        public int port;
        public IPAddress ip;
        public IPEndPoint endpoint;

        public SocketConnection(string hostName, int port)
        {
            ip = GetIp(hostName);
            endpoint = new IPEndPoint(ip, port);
            this.port = port;
        }

        private IPAddress GetIp(string hostName)
        {
            var host = Dns.GetHostEntry((hostName == null)? Dns.GetHostName() : hostName);
            foreach (var ip in host.AddressList)
            {
                if (isLocalIP(ip))
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private bool isLocalIP(IPAddress ip)
        {
            var bytes = ip.GetAddressBytes();
            return ip.AddressFamily == AddressFamily.InterNetwork;
        }
    }
}
