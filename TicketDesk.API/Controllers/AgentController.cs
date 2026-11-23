using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketDesk.API.Services;
using TicketDesk.API.DTOs;

namespace TicketDesk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Agent")]
    public class AgentController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public AgentController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // Get all available tickets for agents.
        [HttpGet("available-tickets")]
        public async Task<IActionResult> GetAvailableTickets()
        {
            var tickets = await _ticketService.GetAvailableTicketsAsync();

            return Ok(tickets);
        }

        [HttpPost("tickets/{ticketId}/pickup")]
        public async Task<IActionResult> PickUpTicket(int ticketId)
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var result = await _ticketService.PickUpTicketAsync(
                ticketId, int.Parse(userId));

            if (!result)
            {
                return Conflict(new
                {
                    message = "Ticket is already assigned or is not available."
                });
            }

            return Ok(new
            {
                message = "Ticket picked up successfully.",
                ticketId = ticketId
            });
        }
        // Agent resolves an assigned ticket.
        [HttpPut("tickets/{ticketId}/resolve")]
        public async Task<IActionResult> ResolveTicket(
            int ticketId, UpdateTicketStatusDto request)
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var result = await _ticketService.ResolveTicketAsync(
                ticketId, int.Parse(userId));

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Ticket not found, not assigned to you, or not in progress."
                });
            }

            return Ok(new
            {
                message = "Ticket resolved successfully.",
                ticketId = ticketId,
                status = request.Status
            });
        }
    }
}