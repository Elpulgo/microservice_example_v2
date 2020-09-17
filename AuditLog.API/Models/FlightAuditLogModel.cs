using System;
using System.Text.Json;
using EventStore.ClientAPI;
using Flights.Core;
using Shared.Core.Models;

namespace AuditLog.API.Models
{
    public class FlightAuditLogModel
    {
        public FlightAuditLogModel()
        { }

        public FlightAuditLogModel(RecordedEvent @event)
        {
            Created = @event.Created;
            EventType = @event.EventType;
            EventNumber = @event.EventNumber;
            EventId = @event.EventId;
            Data = JsonSerializer.Deserialize<Flight>(@event.Data);
            MetaData = JsonSerializer.Deserialize<EventDataMeta>(@event.Metadata);
        }

        public DateTime Created { get; }
        public string EventType { get; }
        public long EventNumber { get; }
        public Guid EventId { get; }
        public Flight Data { get; }
        public EventDataMeta MetaData { get; }
    }
}