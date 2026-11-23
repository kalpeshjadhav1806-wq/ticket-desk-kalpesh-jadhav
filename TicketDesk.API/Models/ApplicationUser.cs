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

        // Stores the hashed password for secure authentication.
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Available roles: Employee, Agent and Manager.
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Tickets created by this user.
        public ICollection<SupportTicket> CreatedTickets { get; set; }
            = new List<SupportTicket>();

        // Tickets assigned to this user.
        public ICollection<SupportTicket> AssignedTickets { get; set; }
            = new List<SupportTicket>();

        // Status changes made by this user.
        public ICollection<TicketStatusHistory> StatusChanges { get; set; }
            = new List<TicketStatusHistory>();
    
}
}
