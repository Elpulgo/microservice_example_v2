using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flights.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Flights.Processor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IFlightEventStoreSubscriber m_Subscriber;

        public Worker(ILogger<Worker> logger, IFlightEventStoreSubscriber subscriber)
        {
            _logger = logger;
            m_Subscriber = subscriber;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await m_Subscriber.Subscribe();
        }
    }
}
