using ConferencesManagementAPI.Constants;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConferencesManagementAPI.Data.DTO
{
    public class ResolveSupportTicketDTO
    {
        public int TicketId { get; set; }
        public string ReplyContext { get; set; } = "";
    }
}
