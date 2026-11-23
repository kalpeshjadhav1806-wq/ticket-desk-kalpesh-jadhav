using System.ComponentModel.DataAnnotations;

namespace TicketDesk.API.Models
{
    public class SupportTicket
    {
      
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(100)]
            public string Title { get; set; } = string.Empty;

            [Required]
            public string Description { get; set; } = string.Empty;

            // Low, Medium or High.
            [Required]
            [MaxLength(20)]
            public string Priority { get; set; } = "Medium";

            // Open, InProgress, Resolved or Closed.
            [Required]
            [MaxLength(20)]
            public string Status { get; set; } = "Open";

            // User who created the ticket.
            public int CreatedBy { get; set; }

            // Agent assigned to the ticket. Can be null.
            public int? AssignedTo { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

            // Stores when the ticket was closed.
            public DateTime? ClosedAt { get; set; }

            // Navigation property for ticket creator.
            public ApplicationUser Creator { get; set; } = null!;

            // Navigation property for assigned agent.
            public ApplicationUser? Assignee { get; set; }

            // Stores the complete status change history.
            public ICollection<TicketStatusHistory> StatusHistory { get; set; }
                = new List<TicketStatusHistory>();
        }
    }

