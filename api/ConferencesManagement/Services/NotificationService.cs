using ConferencesManagementAPI.DAO.Repositories;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;

public class NotificationService
{
    private readonly NotificationRepositories _notificationRepository;

    public NotificationService(NotificationRepositories notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<GeneralResponseDTO> SendNotificationToAllAsync(string message)
    {
        try
        {
            await _notificationRepository.SendNotificationToAllUsersAsync(message);
            await _notificationRepository.SaveChangesAsync();
            return new GeneralResponseDTO { isSuccess = true, Message = "Notification sent to all users successfully" };
        }
        catch (Exception ex)
        {
            return new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<GeneralResponseDTO> SendNotificationToUserAsync(int userId, string message)
    {
        try
        {
            await _notificationRepository.SendNotificationToUserId(userId, message);
            await _notificationRepository.SaveChangesAsync();
            return new GeneralResponseDTO { isSuccess = true, Message = "Notification sent successfully" };
        }
        catch (Exception ex)
        {
            return new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<GeneralResponseDTO> GetAllNotificationsAsync()
    {
        try
        {
            var notifications = await _notificationRepository.GetAllAsync();
            notifications = notifications.OrderByDescending(a => a.CreatedAt);
            return new GeneralResponseDTO { isSuccess = true, Message = "Notifications retrieved successfully", data = notifications };
        }
        catch (Exception ex)
        {
            return new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}", data = new List<Notification>() };
        }
    }

    public async Task<GeneralResponseDTO> GetAllNotificationsForUserAsync(int userId)
    {
        try
        {
            var notifications = await _notificationRepository.GetNotificationsForUserAsync(userId);
            return new GeneralResponseDTO { isSuccess = true, Message = "User notifications retrieved successfully", data = notifications };
        }
        catch (Exception ex)
        {
            return new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}", data = new List<Notification>() };
        }
    }
}
