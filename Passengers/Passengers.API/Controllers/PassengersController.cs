using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Passengers.Application.Commands;
using Passengers.Application.Queries;
using Passengers.Application.Responses;

namespace Passengers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassengersController : ControllerBase
    {
        private readonly IMediator m_Mediator;

        public PassengersController(IMediator mediator)
        {
            m_Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        #region Commands

        [HttpPost]
        [ProducesResponseType(typeof(PassengerCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreatePassenger([FromBody] CreatePassengerCommand command)
            => Ok((await m_Mediator.Send(command)));

        [HttpDelete]
        [ProducesResponseType(typeof(PassengerCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletePassengerById([FromBody] DeletePassengerCommand command)
            => Ok((await m_Mediator.Send(command)));


        [HttpPut]
        [ProducesResponseType(typeof(PassengerCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdatePassengerCommand command)
            => Ok((await m_Mediator.Send(command)));

        #endregion

        #region Queries

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PassengerResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPassengerById(Guid id)
        {
            var result = await m_Mediator.Send(new GetPassengerByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("flight/{flightid}")]
        [ProducesResponseType(typeof(IReadOnlyList<PassengerResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPassengerByFlight(Guid flightId)
        {
            var result = await m_Mediator.Send(new GetFlightPassengersQuery(flightId));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        #endregion
    }
}
