using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketDesk.API.DTOs;
using TicketDesk.API.Services;

namespace TicketDesk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketsController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

      
        [HttpPost]
        public async Task<IActionResult> CreateTicket(
            CreateTicketDto request)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();
            var ticket = await _ticketService.CreateTicketAsync(
                request, int.Parse(userId));

            return StatusCode(201, new
            {
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.Priority,
                ticket.Status,
                ticket.CreatedBy,
                ticket.AssignedTo,
                ticket.CreatedAt,
                ticket.UpdatedAt
            });
        }

        // Employee views their own tickets.
        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var tickets = await _ticketService.GetMyTicketsAsync(
                int.Parse(userId));

            return Ok(tickets);
        }

        // Employee closes their own resolved ticket.
        [HttpPut("{ticketId}/close")]
        public async Task<IActionResult> CloseTicket(int ticketId)
        {
            var employeeId = int.Parse(
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            var result = await _ticketService.CloseTicketAsync(
                ticketId, employeeId);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "You can close only your own resolved tickets."
                });
            }

            return Ok(new
            {
                message = "Ticket closed successfully.",
                ticketId,
                status = "Closed"
            });
        }
    }
}