using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class NotificationRepositories : GenericRepository<Notification>
    {
        private readonly ConferenceManagementDbContext _context;
        public NotificationRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<Notification>> logger) : base(context, logger)
        {
            _context = context;
        }
        public async Task<List<Notification>> GetNotificationsForUserAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId || n.UserId == null)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetAllNotifications()
        {
            return await _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task SendNotificationToAllUsersAsync(string message)
        {
            var notification = new Notification { Message = message };
            await _context.Notifications.AddAsync(notification);
        }

        public async Task SendNotificationToUserId(int userId, string message)
        {
            var notification = new Notification { UserId = userId, Message = message };
            await _context.Notifications.AddAsync(notification);
        }


    }
}
