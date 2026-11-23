using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketDesk.API.Models;

namespace TicketDesk.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.ApplicationUsers.AnyAsync())
                return;

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var users = new[]
            {
            new ApplicationUser
            {
                FullName = "Test Employee",
                Username = "employee",
                Role = "Employee",
                CreatedAt = DateTime.UtcNow
            },
            new ApplicationUser
            {
                FullName = "Test Agent",
                Username = "agent",
                Role = "Agent",
                CreatedAt = DateTime.UtcNow
            },
            new ApplicationUser
            {
                FullName = "Test Manager",
                Username = "manager",
                Role = "Manager",
                CreatedAt = DateTime.UtcNow
            }
        };

            foreach (var user in users)
            {
                var password = user.Role switch
                {
                    "Employee" => "Employee@123",
                    "Agent" => "Agent@123",
                    "Manager" => "Manager@123",
                    _ => throw new InvalidOperationException("Invalid user role.")
                };

                user.PasswordHash = passwordHasher.HashPassword(user, password);
            }

            await context.ApplicationUsers.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}