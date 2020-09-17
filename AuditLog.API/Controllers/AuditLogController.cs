using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditLog.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Events;

namespace AuditLog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IEventStoreReader m_EventStoreReader;
        private IConfiguration Configuration { get; }

        public AuditLogController(
            IEventStoreReader eventStoreReader,
            IConfiguration configuration)
        {
            m_EventStoreReader = eventStoreReader;
            Configuration = configuration;
        }


        [HttpGet("flights")]
        public async Task<IActionResult> GetAuditLogForFlights()
        {
            var flightStreamName = Configuration["EVENTSTORE_FLIGHT_STREAM_NAME"];
            var result = (await m_EventStoreReader.ReadAll(flightStreamName))
                .Select(resolvedEvent => new FlightAuditLogModel(resolvedEvent.Event))
                .ToList();

            return Ok(result);
        }

        [HttpGet("passengers")]
        public async Task<IActionResult> GetAuditLogForPassengers()
        {
            var passengerStreamName = Configuration["EVENTSTORE_PASSENGER_STREAM_NAME"];
            return null;
        }
    }
}
