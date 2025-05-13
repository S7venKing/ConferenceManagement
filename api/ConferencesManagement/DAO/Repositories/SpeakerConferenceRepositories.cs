using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class SpeakerConferenceRepositories : GenericRepository<SpeakerConference>
    {
        private readonly ConferenceManagementDbContext _context;
        public SpeakerConferenceRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<SpeakerConference>> logger) : base(context, logger)
        {
            _context = context;
        }

        public List<SpeakerConferenceDTO> GetSpeakersByConference(int conferenceId)
        {
            var result = _context.SpeakerConference
                .Include(a => a.Conference)
                .Include(a => a.Speaker)
                .Where(a => a.ConferenceId == conferenceId)
                .Select(a => new SpeakerConferenceDTO
                {
                    ConferenceId = a.ConferenceId,
                    ConferenceName = a.Conference.Name,
                    SpeakerId = a.SpeakerId,
                    SpeakerName = a.Speaker.FullName
                })
                .ToList();

            return result;
        }
    }
}
