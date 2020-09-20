using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Flights.Application.Commands;
using Flights.Application.Queries;
using Flights.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Models;

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

        #region Commands

        [HttpPost]
        [ProducesResponseType(typeof(FlightCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateFlight([FromBody] CreateFlightCommand command)
            => Ok((await m_Mediator.Send(command)));


        [HttpDelete]
        [ProducesResponseType(typeof(CommandResponseBase), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteById([FromBody] DeleteFlightCommand command)
            => Ok((await m_Mediator.Send(command)));

        [HttpPut]
        [ProducesResponseType(typeof(CommandResponseBase), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateFlightCommand command)
            => Ok((await m_Mediator.Send(command)));

        #endregion

        #region Queries

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FlightResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFlightById(Guid id)
        {
            var result = await m_Mediator.Send(new GetFlightByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("exists/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> FlightExists(Guid id)
        {
            var result = await m_Mediator.Send(new GetFlightByIdQuery(id));
            if (result == null)
                return Ok(false);

            return Ok(true);
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

        #endregion
    }
}
