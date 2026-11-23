using Microsoft.EntityFrameworkCore;
using TicketDesk.API.Models;

namespace TicketDesk.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Receives database configuration from the application.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Database tables.
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<SupportTicket> SupportTickets { get; set; }

        public DbSet<TicketStatusHistory> TicketStatusHistories { get; set; }

        // Configure table relationships and constraints.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Username must be unique.
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Configure ticket creator relationship.
            modelBuilder.Entity<SupportTicket>()
                .HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure assigned agent relationship.
            modelBuilder.Entity<SupportTicket>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ticket status history relationship.
            modelBuilder.Entity<TicketStatusHistory>()
                .HasOne(h => h.Ticket)
                .WithMany(t => t.StatusHistory)
                .HasForeignKey(h => h.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure user who changed the ticket status.
            modelBuilder.Entity<TicketStatusHistory>()
                .HasOne(h => h.ChangedByUser)
                .WithMany(u => u.StatusChanges)
                .HasForeignKey(h => h.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

