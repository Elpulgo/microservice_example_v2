using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Passengers.Core;

namespace Passengers.Processor
{
    public class Worker : BackgroundService
    {
        private readonly IPassengerEventStoreSubscriber m_Subscriber;

        public Worker(IPassengerEventStoreSubscriber subscriber)
        {
            m_Subscriber = subscriber;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await m_Subscriber.Subscribe();
        }
    }
}
