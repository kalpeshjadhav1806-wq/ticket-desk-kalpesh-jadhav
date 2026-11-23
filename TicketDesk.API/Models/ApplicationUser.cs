using System.ComponentModel.DataAnnotations;

namespace TicketDesk.API.Models
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

     
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

    
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

     
        public ICollection<SupportTicket> CreatedTickets { get; set; }
            = new List<SupportTicket>();

     
        public ICollection<SupportTicket> AssignedTickets { get; set; }
            = new List<SupportTicket>();

       
        public ICollection<TicketStatusHistory> StatusChanges { get; set; }
            = new List<TicketStatusHistory>();
    
}
}
