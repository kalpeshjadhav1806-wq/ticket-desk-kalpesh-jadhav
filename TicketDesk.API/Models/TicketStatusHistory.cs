using System.ComponentModel.DataAnnotations;

namespace TicketDesk.API.Models
{
    public class TicketStatusHistory
    {
        [Key]
        public int Id { get; set; }

        // Related ticket ID.
        public int TicketId { get; set; }

        [Required]
        [MaxLength(20)]
        public string OldStatus { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string NewStatus { get; set; } = string.Empty;

        // User who performed the status change.
        public int ChangedBy { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for the related ticket.
        public SupportTicket Ticket { get; set; } = null!;

        // Navigation property for the user who changed the status.
        public ApplicationUser ChangedByUser { get; set; } = null!;
    }
}
