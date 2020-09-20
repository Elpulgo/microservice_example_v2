using System;
using System.Text.Json;
using EventStore.ClientAPI;
using Passengers.Core.Models;
using Shared.Core.Models;

namespace AuditLog.API.Models
{
    public class PassengerAuditLogModel : IAuditLogModel<Passenger>
    {
        public PassengerAuditLogModel()
        { }

        public PassengerAuditLogModel(RecordedEvent @event)
        {
            Created = @event.Created;
            EventType = @event.EventType;
            EventNumber = @event.EventNumber;
            EventId = @event.EventId;
            Data = JsonSerializer.Deserialize<Passenger>(@event.Data);
            MetaData = JsonSerializer.Deserialize<EventDataMeta>(@event.Metadata);
        }

        public DateTime Created { get; }
        public string EventType { get; }
        public long EventNumber { get; }
        public Guid EventId { get; }
        public Passenger Data { get; }
        public EventDataMeta MetaData { get; }
    }
}