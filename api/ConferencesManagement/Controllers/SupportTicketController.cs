using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementAPI.Services;
using ConferencesManagementAPI.Utils;
using Microsoft.AspNetCore.Mvc;

[Route("api/support-tickets")]
[ApiController]
public class SupportTicketController : ControllerBase
{
    private readonly SupportTicketService _ticketService;

    public SupportTicketController(SupportTicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet("get-all")]
    [AuthorizeAdmin]
    public async Task<IActionResult> GetAllTickets()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();
        return Ok(new GeneralResponseDTO
        {
            isSuccess = true,
            Message = "Success",
            data = tickets
        });
    }

    [HttpGet("get-by-me")]
    [AuthorizeUser]
    public async Task<IActionResult> GetAllTicketByMe()
    {
        var userId = JwtHelper.GetUserIdFromToken(HttpContext);
        if (userId == null)
            return Unauthorized("Invalid Token");
        var tickets = await _ticketService.GetByUserId(userId.Value);
        return Ok(new GeneralResponseDTO
        {
            isSuccess = true,
            Message = "Success",
            data = tickets
        });
    }

    [HttpGet("get-by-id")]
    [AuthorizeAdmin]
    public async Task<IActionResult> GetTicketById([FromQuery] int id)
    {

        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null) return NotFound();
        return Ok(ticket);
    }

    [HttpPost("create-tickets")]
    [AuthorizeUser]
    public async Task<IActionResult> CreateTicket([FromBody] CreateSupportTicketRequest request)
    {
        if (string.IsNullOrEmpty(request.Subject) || string.IsNullOrEmpty(request.Description))
            return BadRequest("Subject and Description are required.");

        var userId = JwtHelper.GetUserIdFromToken(HttpContext);
        if (userId == null)
            return Unauthorized("Invalid Token");


        await _ticketService.CreateTicketAsync(userId.Value, request.Subject, request.Description);
        return Ok(new { message = "Support ticket created successfully" });
    }

    [HttpPost("resolve-by-admin")]
    [AuthorizeAdmin]
    public async Task<IActionResult> ResolveTicket(ResolveSupportTicketDTO resolveSupportTicketDTO)
    {
        var success = await _ticketService.ResolveTicketAsync(resolveSupportTicketDTO.TicketId, resolveSupportTicketDTO.ReplyContext);
        if (!success) return NotFound("Ticket not found.");
        return Ok(new { message = "Ticket resolved successfully" });
    }

    [HttpPost("delete-by-admin")]
    [AuthorizeAdmin]
    public async Task<IActionResult> DeleteTicket([FromQuery] int id)
    {
        var userId = JwtHelper.GetUserIdFromToken(HttpContext);
        if (userId == null)
            return Unauthorized("Invalid Token");
        var success = await _ticketService.DeleteTicketAsync(id);
        if (!success) return NotFound("Ticket not found.");
        return Ok(new { message = "Ticket deleted successfully" });
    }
}

public class CreateSupportTicketRequest
{
    public string Subject { get; set; } = "Đơn Hỗ trợ";
    public string Description { get; set; } = "Chi tiết";
}
