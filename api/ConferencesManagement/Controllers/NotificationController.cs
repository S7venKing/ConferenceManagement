using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementAPI.Utils;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("send-to-all")]
    [AuthorizeAdmin]
    public async Task<IActionResult> SendNotificationToAll([FromBody] SendNotificationRequestDTO request)
    {
        try
        {
            var response = await _notificationService.SendNotificationToAllAsync(request.Message);
            return response.isSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new GeneralResponseDTO
            {
                isSuccess = false,
                Message = $"Internal Server Error: {ex.Message}"
            });
        }
    }

    [HttpPost("send-to-user")]
    [AuthorizeAdmin]
    public async Task<IActionResult> SendNotificationToUser([FromBody] SendNotificationToUserRequestDTO request)
    {
        try
        {
            var response = await _notificationService.SendNotificationToUserAsync(request.DelegateId, request.Message);
            return response.isSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new GeneralResponseDTO
            {
                isSuccess = false,
                Message = $"Internal Server Error: {ex.Message}"
            });
        }
    }

    [HttpGet("get-all")]
    [AuthorizeUser]
    public async Task<IActionResult> GetAllNotifications()
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromToken(HttpContext);
            if (userId == null)
            {
                return Unauthorized(new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = "Invalid Token"
                });
            }

            var response = await _notificationService.GetAllNotificationsForUserAsync(userId.Value);
            return response.isSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new GeneralResponseDTO
            {
                isSuccess = false,
                Message = $"Internal Server Error: {ex.Message}"
            });
        }
    }

    [HttpGet("get-all-by-admin")]
    [AuthorizeAdmin]
    public async Task<IActionResult> GetAllNotificationsByAdmin()
    {
        try
        {
            var response = await _notificationService.GetAllNotificationsAsync();
            return response.isSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new GeneralResponseDTO
            {
                isSuccess = false,
                Message = $"Internal Server Error: {ex.Message}"
            });
        }
    }
}
