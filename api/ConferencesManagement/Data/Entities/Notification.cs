namespace ConferencesManagementDAO.Data.Entities;
public class Notification
{
    public int Id { get; set; }
    public int? UserId { get; set; } // Nullable: nếu null thì là thông báo chung
    public string Message { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
