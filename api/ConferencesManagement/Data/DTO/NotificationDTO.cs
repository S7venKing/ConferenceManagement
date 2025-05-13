public class SendNotificationRequestDTO
{
    public string Message { get; set; } = "";
}

public class SendNotificationToUserRequestDTO
{
    public int DelegateId { get; set; }
    public string Message { get; set; } = "";
}
