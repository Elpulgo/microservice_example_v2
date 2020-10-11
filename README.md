# Microservice-architecture
An example of domain driven microservice architecture with event sourcing & CQRS in docker containers.
Use **docker-compose.prod.yml** to test it out. Images are available at **https://hub.docker.com/u/elpulgo**.
Serves an application where the user can handle flights and passengers, and in realtime see the auditlog, 
based on an event store. 

## Scenarios
+ Create flight
+ Add passanger to flight
+ Board passenger
+ Disembark flight (all passengers must have boarded)
+ Land flight
+ Delete flight (passengers are deleted as well)

## Architecture

<img src="https://github.com/Elpulgo/microservice_example_v2/blob/master/documentation/architecture.png" width="640">

## Flow
 - **Flight**
    - Create/update/delete flight from client
    - Flight API handles command, via mediator
    - Command handler validates command
        - If update/delete command, Passenger API is notified/requested through JSON RPC call
    - Append event in stream 'flight-stream' in EventStoreDB
    - Flight Processor subscribe to 'flight-stream' events in EventStoreDB
    - Flight Processor change state of entity in Postgre SQL database
- **Passenger**
    - Create/update passenger from client
    - Passenger API handles command, via mediator
    - Command handler validates command
        - If create/update, Flight API is notified/requested through JSON RPC call
    - Append event in stream 'passenger-stream' in EventStoreDB
    - Passenger Processor subscribe to 'passenger-stream' in EventStoreDB
    - Passenger Processor change state of entity in Postgre SQL database
- **Audit Log**
    - Fetch audit logs on a timer from client
    - Auditlog API handles GET request and read all events from EventStoreDB

## Technologies / services

    - Client                    / Angular
    - API-Gateway               / NetCore WebApi / Ocelot
    - Flights API               / NetCore WebApi / CQRS w MediatR / JSON-RPC
    - Flights Processor         / Net Core Worker
    - Flights read database     / Postgre SQL
    - Passengers API            / NetCore WebApi / CQRS w MediatR / JSON-RPC
    - Passengers Processor      / Net Core Worker
    - Passenger read database   / Postgre SQL
    - Event store               / EventStoreDB
    - Auditlog API              / NetCore WebApi

## Description

Serves as an example of how to compose a microservice architecture with a domain driven design.
The project is done with a goal of learning event sourcing and CQRS pattern.
The project is structured around the domains 'Flight' and 'Passenger'.

I have investigated topics such as CQRS (with MediatR), RPC, Event sourcing among others to be able to
build this architecture.

The project is structured around the different domains, i.e 'Flights' and 'Passengers'.
Each domain uses generic implementations from a shared dependency, like database connections/operations, RPC server host/client and operations, 
event store connections/operations.

Each domain is structured with the same template
- x.API             NetCore webapi, which host the server for the specified domain
- x.Application     CQRS related logic, commands, queries, RPC implementations etc.
- x.Core            Models, extensions, repository interfaces, mappings, events
- x.Infrastructure  Event store related implementations, read/write database repository implementations
- x.Processor       Worker for handling subscriptions from event store

Images provided by me can be found on docker hub, and in the root directory there is a docker-compose.prod.yml to be used if you 
don't want to use the source code.