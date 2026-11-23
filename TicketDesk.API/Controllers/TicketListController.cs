using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketDesk.API.Services;

namespace TicketDesk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee,Agent,Manager")]
    public class TicketListController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketListController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // Get tickets with filtering and pagination.
        [HttpGet]
        public async Task<IActionResult> GetTickets(
            string? status,
            string? priority,
            int page = 1,
            int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new
                {
                    message = "Page must be at least 1 and pageSize must be between 1 and 100."
                });
            }

            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var role = User.FindFirst(ClaimTypes.Role)!.Value;

            var result = await _ticketService.GetTicketsAsync(
                role,
                userId,
                status,
                priority,
                page,
                pageSize);

            return Ok(result);
        }
    }
}