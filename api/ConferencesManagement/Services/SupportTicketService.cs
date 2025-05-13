using ConferencesManagementAPI.DAO.Repositories;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.Extensions.Logging;

namespace ConferencesManagementAPI.Services
{
    public class SupportTicketService
    {
        private readonly SupportTicketRepositories _ticketRepository;
        private readonly ILogger<SupportTicketService> _logger;

        public SupportTicketService(SupportTicketRepositories repository, ILogger<SupportTicketService> logger)
        {
            _ticketRepository = repository;
            _logger = logger;
        }
        public async Task<IEnumerable<SupportTicket>> GetAllTicketsAsync()
        {
            return await _ticketRepository.GetAllTicketsAsync();
        }

        public async Task<SupportTicket?> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetTicketByIdAsync(id);
        }

        public async Task<bool> CreateTicketAsync(int userId, string subject, string description)
        {
            var ticket = new SupportTicket
            {
                UserId = userId,
                Subject = subject,
                Description = description
            };

            await _ticketRepository.AddTicketAsync(ticket);
            return true;
        }

        public async Task<bool> ResolveTicketAsync(int ticketId, string ResolveContext)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            if (ticket == null) return false;

            ticket.IsResolved = true;
            ticket.ResolvedAt = DateTime.UtcNow;
            ticket.ResolvedReplyContext = ResolveContext;
            await _ticketRepository.UpdateTicketAsync(ticket);
            return true;
        }

        public async Task<bool> DeleteTicketAsync(int ticketId)
        {
            await _ticketRepository.DeleteTicketAsync(ticketId);
            return true;
        }

        public async Task<IEnumerable<SupportTicket>> GetByUserId(int userId)
        {
            var result = await _ticketRepository.FindAsync(a => a.UserId == userId);
            return result;
        }
    }


}
