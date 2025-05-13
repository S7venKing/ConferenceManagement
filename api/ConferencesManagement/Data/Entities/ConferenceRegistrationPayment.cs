using ConferencesManagementDAO.Data.Entities;

public class ConferenceRegistrationPayment
{
    public int Id { get; set; }
    public int RegistrationId { get; set; }  
    public decimal Amount { get; set; }  
    public DateTime PaymentDate { get; set; }  
    public string? PaymentStatus { get; set; }  

    // Khóa ngoại liên kết với bảng Registration
    public virtual Registration? Registration { get; set; }
}
