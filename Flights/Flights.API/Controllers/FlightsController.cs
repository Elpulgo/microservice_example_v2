﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Flights.Application.Commands;
using Flights.Application.Queries;
using Flights.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Flights.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IMediator m_Mediator;

        public FlightsController(IMediator mediator)
        {
            m_Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType(typeof(FlightCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateFlight([FromBody] CreateFlightCommand command)
        {
            var result = await m_Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FlightResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFlightById(Guid id)
        {
            var result = await m_Mediator.Send(new GetFlightByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<FlightResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllFlights()
        {
            var result = await m_Mediator.Send(new GetAllFlightsQuery());
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
