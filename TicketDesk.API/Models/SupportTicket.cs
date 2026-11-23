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
            [Required]
            [MaxLength(20)]
            public string Priority { get; set; } = "Medium";

         
            [Required]
            [MaxLength(20)]
            public string Status { get; set; } = "Open";

   
            public int CreatedBy { get; set; }

           
            public int? AssignedTo { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

           
            public DateTime? ClosedAt { get; set; }

          
            public ApplicationUser Creator { get; set; } = null!;

          
            public ApplicationUser? Assignee { get; set; }
            public ICollection<TicketStatusHistory> StatusHistory { get; set; }
                = new List<TicketStatusHistory>();
        }
    }

