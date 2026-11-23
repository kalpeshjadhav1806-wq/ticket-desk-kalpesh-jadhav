using System.ComponentModel.DataAnnotations;

namespace TicketDesk.API.DTOs
{
    public class CreateTicketDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Low|Medium|High)$",
            ErrorMessage = "Priority must be Low, Medium or High.")]
        public string Priority { get; set; } = "Medium";
    }
}