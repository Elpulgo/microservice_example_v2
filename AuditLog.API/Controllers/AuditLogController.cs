using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditLog.API.Models;
using Microsoft.AspNetCore.Mvc;
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

        public AuditLogController(IEventStoreReader eventStoreReader)
        {
            m_EventStoreReader = eventStoreReader;
        }

        [HttpGet("flights")]
        public async Task<IActionResult> GetAuditLogForFlights()
        {
            var result = (await m_EventStoreReader.ReadAll("event_flights"))
                .Select(resolvedEvent => new FlightAuditLogModel(resolvedEvent.Event))
                .ToList();

            return Ok(result);
        }

        [HttpGet("passengers")]
        public async Task<IActionResult> GetAuditLogForPassengers()
        {
            return null;
        }

    }
}
