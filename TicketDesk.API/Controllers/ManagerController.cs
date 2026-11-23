using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketDesk.API.Services;


namespace TicketDesk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public ManagerController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

     
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _ticketService.GetManagerSummaryAsync();

            return Ok(summary);
        }
 
        [HttpGet("tickets/{ticketId}/history")]
        public async Task<IActionResult> GetTicketHistory(int ticketId)
        {
            var history = await _ticketService.GetTicketHistoryAsync(ticketId);

            return Ok(history);
        }

  
        [HttpPut("tickets/{ticketId}/reopen")]
        public async Task<IActionResult> ReopenTicket(int ticketId)
        {
            var managerId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            var result = await _ticketService.ReopenTicketAsync(ticketId, managerId);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Ticket cannot be reopened. It may not be closed or may have been closed more than 7 days ago."
                });
            }

            return Ok(new
            {
                message = "Ticket reopened successfully.",
                ticketId = ticketId,
                status = "Open"
            });
        }
      
    }
}