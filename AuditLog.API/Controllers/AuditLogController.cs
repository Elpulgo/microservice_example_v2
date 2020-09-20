using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditLog.API.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Events;

namespace AuditLog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IEventStoreReader m_EventStoreReader;
        private EventStoreConfiguration EventStoreConfiguration { get; }

        public AuditLogController(
            IEventStoreReader eventStoreReader,
            EventStoreConfiguration eventStoreConfiguration)
        {
            m_EventStoreReader = eventStoreReader;
            EventStoreConfiguration = eventStoreConfiguration;
        }


        [HttpGet("flights")]
        public async Task<IActionResult> GetAuditLogForFlights()
            => Ok((await GetAllFlights()));

        [HttpGet("passengers")]
        public async Task<IActionResult> GetAuditLogForPassengers()
            => Ok((await GetAllPassengers()));

        [HttpGet("all")]
        public async Task<IActionResult> Test()
        {
            var passengers = await GetAllPassengers();
            var flights = await GetAllFlights();

            var all = new List<IEnumerable<dynamic>> { passengers, flights }
                .SelectMany(s => s)
                .OrderBy(s => s.Created).ToList();

            return Ok(all);
        }

        private async Task<List<PassengerAuditLogModel>> GetAllPassengers()
            => (await m_EventStoreReader.ReadAll(EventStoreConfiguration.PassengerStreamName))
               .Select(resolvedEvent => new PassengerAuditLogModel(resolvedEvent.Event))
               .ToList();

        private async Task<List<FlightAuditLogModel>> GetAllFlights()
            => (await m_EventStoreReader.ReadAll(EventStoreConfiguration.FlightStreamName))
                .Select(resolvedEvent => new FlightAuditLogModel(resolvedEvent.Event))
                .ToList();
    }
}
