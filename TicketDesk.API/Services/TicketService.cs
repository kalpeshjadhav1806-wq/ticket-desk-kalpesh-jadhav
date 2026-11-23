using Microsoft.EntityFrameworkCore;
using TicketDesk.API.Data;
using TicketDesk.API.DTOs;
using TicketDesk.API.Models;

namespace TicketDesk.API.Services
{
    public class TicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<SupportTicket> CreateTicketAsync(
            CreateTicketDto request, int employeeId)
        {
            var ticket = new SupportTicket
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                Status = "Open",
                CreatedBy = employeeId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SupportTickets.Add(ticket);

            await _context.SaveChangesAsync();

          
            var history = new TicketStatusHistory
            {
                TicketId = ticket.Id,
                OldStatus = "None",
                NewStatus = "Open",
                ChangedBy = employeeId,
                ChangedAt = DateTime.UtcNow
            };

            _context.TicketStatusHistories.Add(history);

            await _context.SaveChangesAsync();

            return ticket;
        }

       
        public async Task<List<SupportTicket>> GetMyTicketsAsync(
            int employeeId)
        {
            return await _context.SupportTickets
                .Where(t => t.CreatedBy == employeeId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<SupportTicket>> GetAvailableTicketsAsync()
        {
            return await _context.SupportTickets
                .Where(t => t.Status == "Open" && t.AssignedTo == null)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new SupportTicket
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    CreatedBy = t.CreatedBy,
                    AssignedTo = t.AssignedTo,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();
        }
    
 
public async Task<bool> PickUpTicketAsync(int ticketId, int agentId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var currentTime = DateTime.UtcNow;

           
            var updatedRows = await _context.SupportTickets
                .Where(t => t.Id == ticketId
                            && t.Status == "Open"
                            && t.AssignedTo == null)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.AssignedTo, agentId)
                    .SetProperty(t => t.Status, "InProgress")
                    .SetProperty(t => t.UpdatedAt, currentTime));

         
            if (updatedRows == 0)
            {
                await transaction.RollbackAsync();
                return false;
            }

           
            var history = new TicketStatusHistory
            {
                TicketId = ticketId,
                OldStatus = "Open",
                NewStatus = "InProgress",
                ChangedBy = agentId,
                ChangedAt = currentTime
            };

            _context.TicketStatusHistories.Add(history);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        public async Task<bool> ResolveTicketAsync(int ticketId, int agentId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var ticket = await _context.SupportTickets
                .FirstOrDefaultAsync(t =>
                    t.Id == ticketId &&
                    t.AssignedTo == agentId &&
                    t.Status == "InProgress");

       
            if (ticket == null)
            {
                return false;
            }

            var currentTime = DateTime.UtcNow;


            ticket.Status = "Resolved";
            ticket.UpdatedAt = currentTime;

          
            var history = new TicketStatusHistory
            {
                TicketId = ticketId,
                OldStatus = "InProgress",
                NewStatus = "Resolved",
                ChangedBy = agentId,
                ChangedAt = currentTime
            };

            _context.TicketStatusHistories.Add(history);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }

       
        public async Task<object> GetManagerSummaryAsync()
        {
         
            var openCount = await _context.SupportTickets
                .CountAsync(t => t.Status == "Open");

            var inProgressCount = await _context.SupportTickets
                .CountAsync(t => t.Status == "InProgress");

            var resolvedCount = await _context.SupportTickets
                .CountAsync(t => t.Status == "Resolved");

            var closedCount = await _context.SupportTickets
                .CountAsync(t => t.Status == "Closed");

          
            var overdueDate = DateTime.UtcNow.AddHours(-48);

            var overdueCount = await _context.SupportTickets
                .CountAsync(t =>
                    t.CreatedAt < overdueDate &&
                    t.Status != "Resolved" &&
                    t.Status != "Closed");

            return new
            {
                TotalTickets = openCount + inProgressCount + resolvedCount + closedCount,

                TicketsByStatus = new
                {
                    Open = openCount,
                    InProgress = inProgressCount,
                    Resolved = resolvedCount,
                    Closed = closedCount
                },

                OverdueTickets = overdueCount
            };
        }
     
        public async Task<object> GetTicketHistoryAsync(int ticketId)
        {
            var history = await _context.TicketStatusHistories
                .Where(h => h.TicketId == ticketId)
                .Include(h => h.ChangedByUser)
                .OrderByDescending(h => h.ChangedAt)
                .Select(h => new
                {
                    h.Id,
                    h.TicketId,
                    h.OldStatus,
                    h.NewStatus,
                    ChangedBy = h.ChangedByUser.FullName,
                    h.ChangedAt
                })
                .ToListAsync();

            return history;
        }

      
        public async Task<bool> ReopenTicketAsync(int ticketId, int managerId)
        {
            var ticket = await _context.SupportTickets
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
                return false;

         
            if (ticket.Status != "Closed" || ticket.ClosedAt == null)
                return false;

      
            if (ticket.ClosedAt < DateTime.UtcNow.AddDays(-7))
                return false;

            ticket.Status = "Open";
            ticket.ClosedAt = null;
            ticket.UpdatedAt = DateTime.UtcNow;

            _context.TicketStatusHistories.Add(new TicketStatusHistory
            {
                TicketId = ticket.Id,
                OldStatus = "Closed",
                NewStatus = "Open",
                ChangedBy = managerId,
                ChangedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return true;
        }

      
        public async Task<bool> CloseTicketAsync(int ticketId, int employeeId)
        {
            var ticket = await _context.SupportTickets
                .FirstOrDefaultAsync(t =>
                    t.Id == ticketId &&
                    t.CreatedBy == employeeId);

            if (ticket == null || ticket.Status != "Resolved")
                return false;

            ticket.Status = "Closed";
            ticket.ClosedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;

            _context.TicketStatusHistories.Add(new TicketStatusHistory
            {
                TicketId = ticket.Id,
                OldStatus = "Resolved",
                NewStatus = "Closed",
                ChangedBy = employeeId,
                ChangedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<object> GetTicketsAsync(
            string role,
            int userId,
            string? status,
            string? priority,
            int page,
            int pageSize)
        {
            var query = _context.SupportTickets.AsQueryable();

     
            if (role == "Employee")
            {
                query = query.Where(t => t.CreatedBy == userId);
            }

  
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status == status);
            }

       
            if (!string.IsNullOrEmpty(priority))
            {
                query = query.Where(t => t.Priority == priority);
            }

            var totalRecords = await query.CountAsync();

            var tickets = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Priority,
                    t.Status,
                    t.CreatedBy,
                    t.AssignedTo,
                    t.CreatedAt,
                    t.UpdatedAt,
                    t.ClosedAt
                })
                .ToListAsync();

            return new
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    totalRecords / (double)pageSize),
                Tickets = tickets
            };
        }
    }

}