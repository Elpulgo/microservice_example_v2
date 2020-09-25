using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Hosting;
using Shared.Core.RPC;
using Shared.Infrastructure.Extensions;
using StreamJsonRpc;

namespace Shared.Infrastructure.RPC
{
    public class StreamJsonRcpHost : BackgroundService
    {
        private readonly IStreamJsonRpcServer m_StreamJsonRpcServer;
        private readonly IConnectionListenerFactory m_ConnectionListenerFactory;
        private readonly int m_Port;
        private readonly ConcurrentDictionary<string, (ConnectionContext Context, Task ExecutionTask)> m_Connections
            = new ConcurrentDictionary<string, (ConnectionContext, Task)>();
        private IConnectionListener m_ConnectionListener;

        public StreamJsonRcpHost(
            IStreamJsonRpcServer streamJsonRpcServer,
            IConnectionListenerFactory connectionListenerFactory,
            int port)
        {
            m_StreamJsonRpcServer = streamJsonRpcServer;
            m_ConnectionListenerFactory = connectionListenerFactory ?? throw new ArgumentNullException(nameof(connectionListenerFactory));
            m_Port = port != 0 ? port : throw new ArgumentException($"{nameof(port)} can't be 0");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var dockerContainerIpAddress = RpcExtensions.GetDockerIpAddress();

            m_ConnectionListener = await m_ConnectionListenerFactory.BindAsync(
                new IPEndPoint(dockerContainerIpAddress, m_Port),
                stoppingToken);

            Console.WriteLine($"RPC server setup for '{dockerContainerIpAddress}:{m_Port}'.");

            while (true)
            {
                var connectionContext = await m_ConnectionListener.AcceptAsync(stoppingToken);

                // AcceptAsync will return null upon disposing the listener
                if (connectionContext == null)
                    break;

                m_Connections[connectionContext.ConnectionId] = (connectionContext, AcceptAsync(connectionContext));
            }

            var connectionsExecutionTasks = new List<Task>(m_Connections.Count);

            foreach (var connection in m_Connections)
            {
                connectionsExecutionTasks.Add(connection.Value.ExecutionTask);
                connection.Value.Context.Abort();
            }

            await Task.WhenAll(connectionsExecutionTasks);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
            => await m_ConnectionListener.DisposeAsync();

        private async Task AcceptAsync(ConnectionContext connectionContext)
        {
            try
            {
                await Task.Yield();

                Console.WriteLine($"Connection {connectionContext.ConnectionId} connected");

                var jsonRpcMessageFormatter = new JsonMessageFormatter(Encoding.UTF8);

                var jsonRpcMessageHandler = new LengthHeaderMessageHandler(
                    connectionContext.Transport,
                    jsonRpcMessageFormatter);

                using (var jsonRpc = new JsonRpc(jsonRpcMessageHandler, m_StreamJsonRpcServer))
                {
                    jsonRpc.StartListening();
                    await jsonRpc.Completion;
                }
            }
            catch (ConnectionResetException connectionResetException)
            {
                Console.WriteLine($"ConnectionResetException: '{connectionResetException.Message}'");
            }
            catch (ConnectionAbortedException connectionAbortedException)
            {
                Console.WriteLine($"Connectionabortedexception: '{connectionAbortedException.Message}'");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Connection {connectionContext.ConnectionId} threw an exception: {e.Message}");
            }
            finally
            {
                await connectionContext.DisposeAsync();
                m_Connections.TryRemove(connectionContext.ConnectionId, out _);
                Console.WriteLine($"Connection {connectionContext.ConnectionId} disconnected");
            }
        }
    }
}