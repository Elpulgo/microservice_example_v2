using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Shared.Infrastructure.Extensions
{
    public static class RpcExtensions
    {
        public static IPAddress GetDockerIpAddress()
        {
            var hostName = Dns.GetHostName();

            var dockerContainerIpAddress = Dns.GetHostEntry(hostName)
                .AddressList
                .FirstOrDefault(ad => ad.AddressFamily == AddressFamily.InterNetwork);

            if (dockerContainerIpAddress == null)
                throw new ArgumentNullException(nameof(dockerContainerIpAddress));

            return dockerContainerIpAddress;
        }
    }
}