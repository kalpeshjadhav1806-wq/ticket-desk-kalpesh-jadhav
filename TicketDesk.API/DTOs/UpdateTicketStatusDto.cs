using System.ComponentModel.DataAnnotations;

namespace TicketDesk.API.DTOs
{
    public class UpdateTicketStatusDto
    {
        [Required]
        [RegularExpression("^(Resolved)$",
            ErrorMessage = "Agent can only update status to Resolved.")]
        public string Status { get; set; } = string.Empty;
    }
}