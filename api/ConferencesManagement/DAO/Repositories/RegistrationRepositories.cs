using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class RegistrationRepositories : GenericRepository<Registration>
    {
        private readonly ConferenceManagementDbContext _context;
        public RegistrationRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<Registration>> logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<Registration?> GetByConferenceIdAndDelegateIdAsync(int delegateId, int conferenceId)
        {
            return await _context.Registrations.FirstOrDefaultAsync(a => a.ConferenceId == conferenceId && a.DelegateId == delegateId);
        }

        public List<RegistrationResponseDTO> GetByConferenceId(int conferenceId)
        {
            var registrations = (from r in _context.Registrations
                                 join c in _context.Conferences
                                 on r.ConferenceId equals c.Id
                                 join rp in _context.ConferenceRegistrationPayments
                                      on r.Id equals rp.RegistrationId
                                      into rpGroup
                                 from rp in rpGroup.DefaultIfEmpty()
                                 join dcr in _context.DelegateConferenceRoles
                                     on new { r.DelegateId, r.ConferenceId }
                                     equals new { dcr.DelegateId, dcr.ConferenceId } into dcrGroup
                                 from dcr in dcrGroup.DefaultIfEmpty() // LEFT JOIN

                                 join cr in _context.ConferenceRoles
                                     on dcr.RoleId equals cr.Id into crGroup
                                 from cr in crGroup.DefaultIfEmpty() // LEFT JOIN với ConferenceRoles

                                 where r.ConferenceId == conferenceId
                                 select new RegistrationResponseDTO
                                 {
                                     Id = r.Id,
                                     ConferenceId = r.ConferenceId,
                                     DelegateId = r.DelegateId,
                                     ConferenceRoleId = cr != null ? cr.Id : 3,
                                     ConferenceName = r.Conference != null ? r.Conference.Name : "",
                                     DelegateName = r.Delegate != null ? r.Delegate.FullName : "",
                                     DelegateEmail = r.Delegate != null ? r.Delegate.Email : "",
                                     RegisteredAt = r.RegisteredAt,
                                     Status = r.Status,
                                     ConferenceRole = cr != null ? cr.Name : "Khách mời",
                                     NeedPayFee = c.RegistrationFee > 0,
                                     IsPayFee = rp != null
                                 })
                                 .GroupBy(x => x.Id)  // Nhóm theo Id để loại trùng
                                 .Select(g => g.First()) // Chỉ lấy bản ghi đầu tiên trong nhóm
                                 .ToList();


            return registrations;

        }

        public async Task<ConferenceRegistrationPayment?> GetRegistrationPaymentStatus(int registrationId)
        {
            return await _context.ConferenceRegistrationPayments.FirstOrDefaultAsync(a => a.Id == registrationId);
        }
    }
}
