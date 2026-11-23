using System.ComponentModel.DataAnnotations;

namespace TicketDesk.API.Models
{
    public class TicketStatusHistory
    {
        [Key]
        public int Id { get; set; }

    
        public int TicketId { get; set; }

        [Required]
        [MaxLength(20)]
        public string OldStatus { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string NewStatus { get; set; } = string.Empty;

       
        public int ChangedBy { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    
        public SupportTicket Ticket { get; set; } = null!;

     
        public ApplicationUser ChangedByUser { get; set; } = null!;
    }
}
