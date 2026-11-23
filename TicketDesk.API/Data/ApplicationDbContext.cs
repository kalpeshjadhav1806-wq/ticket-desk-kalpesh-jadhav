using Microsoft.EntityFrameworkCore;
using TicketDesk.API.Models;

namespace TicketDesk.API.Data
{
    public class ApplicationDbContext : DbContext
    {
     
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

 
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<SupportTicket> SupportTickets { get; set; }

        public DbSet<TicketStatusHistory> TicketStatusHistories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Username)
                .IsUnique();


            modelBuilder.Entity<SupportTicket>()
                .HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<SupportTicket>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

         
            modelBuilder.Entity<TicketStatusHistory>()
                .HasOne(h => h.Ticket)
                .WithMany(t => t.StatusHistory)
                .HasForeignKey(h => h.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

         
            modelBuilder.Entity<TicketStatusHistory>()
                .HasOne(h => h.ChangedByUser)
                .WithMany(u => u.StatusChanges)
                .HasForeignKey(h => h.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

