using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class SupportTicketRepositories : GenericRepository<SupportTicket>
    {
        private readonly ConferenceManagementDbContext _context;
        public SupportTicketRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<SupportTicket>> logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupportTicket>> GetAllTicketsAsync()
        {
            return await _context.SupportTickets.ToListAsync();
        }

        public async Task<SupportTicket?> GetTicketByIdAsync(int id)
        {
            return await _context.SupportTickets.FindAsync(id);
        }

        public async Task AddTicketAsync(SupportTicket ticket)
        {
            await _context.SupportTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketAsync(SupportTicket ticket)
        {
            _context.SupportTickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _context.SupportTickets.FindAsync(id);
            if (ticket != null)
            {
                _context.SupportTickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }
    }

}
