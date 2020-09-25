using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using StreamJsonRpc;

namespace Shared.Infrastructure.RPC
{
    public class RpcClient
    {
        private const int MaxConnectionRetries = 10;
        private readonly string m_ServerAddress;
        private readonly int m_Port;

        public RpcClient(string serverAddress, int port)
        {
            m_ServerAddress = !string.IsNullOrEmpty(serverAddress) ? serverAddress : throw new ArgumentNullException(nameof(serverAddress));
            m_Port = port != 0 ? port : throw new ArgumentException($"{nameof(serverAddress)} can't be 0");
        }

        public async Task<LengthHeaderMessageHandler> GetMessageHandlerAsync()
        {
            var tries = 0;

            do
            {
                try
                {
                    var tcpClient = new TcpClient(m_ServerAddress, m_Port);

                    var jsonStream = tcpClient.GetStream();

                    var jsonRpcMessageFormatter = new JsonMessageFormatter(Encoding.UTF8);

                    var jsonRpcMessageHandler = new LengthHeaderMessageHandler(
                        jsonStream,
                        jsonStream,
                        jsonRpcMessageFormatter);

                    return jsonRpcMessageHandler;
                }
                catch (SocketException socketException)
                {
                    Console.WriteLine($"Failed to connect, will retry {tries}/{MaxConnectionRetries}, '{socketException.Message}'");
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Failed to connect, will retry {tries}/{MaxConnectionRetries}, '{exception.Message}'");
                }
                finally
                {
                    tries++;
                    await Task.Delay(1000);
                }
            } while (tries < MaxConnectionRetries);

            throw new TimeoutException("Failed to connect to the RPC server.");
        }
    }
}