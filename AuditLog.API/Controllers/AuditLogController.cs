using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure.Data;

namespace AuditLog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IEventStoreContext m_Context;

        public AuditLogController(IEventStoreContext context)
        {
            m_Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogForFlights()
        {
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogForPassengers()
        {
            return null;
        }

    }
}
