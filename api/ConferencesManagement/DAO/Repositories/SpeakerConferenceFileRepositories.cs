using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class SpeakerConferenceFileRepositories : GenericRepository<SpeakerConferenceFile>
    {
        private readonly ConferenceManagementDbContext _context;
        public SpeakerConferenceFileRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<SpeakerConferenceFile>> logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<SpeakerConferenceFile?> GetFileByIdAsync(int fileId)
        {
            return await _context.SpeakerConferenceFile.FindAsync(fileId);
        }

        public async Task<List<SpeakerConferenceFile>> GetFilesByConferenceIdAsync(int conferenceId)
        {
            return await _context.SpeakerConferenceFile
                .Include(f => f.Speaker)
                .Where(f => f.ConferenceId == conferenceId)
                .ToListAsync();
        }

        public async Task AddFileAsync(SpeakerConferenceFile file)
        {
            await _context.SpeakerConferenceFile.AddAsync(file);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
