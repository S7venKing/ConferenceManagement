using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class ConferenceRegistrationPaymentRepositories : GenericRepository<ConferenceRegistrationPayment>
    {
        private readonly ConferenceManagementDbContext _context;
        public ConferenceRegistrationPaymentRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<ConferenceRegistrationPayment>> logger) : base(context, logger)
        {
            _context = context;
        }

    }
}
