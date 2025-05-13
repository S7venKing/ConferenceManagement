namespace ConferencesManagementDAO.Data.Entities
{
    public class SupportTicket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; } = "Đơn hỗ trợ";
        public string Description { get; set; } = "Chi tiết";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
        public bool IsResolved { get; set; } = false;
        public string? ResolvedReplyContext { get; set; }
    }
}
