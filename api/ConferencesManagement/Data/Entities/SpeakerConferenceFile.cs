using ConferencesManagementDAO.Data.Entities;

public class SpeakerConferenceFile
{
    public int Id { get; set; }
    public int SpeakerId { get; set; }
    public int ConferenceId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Delegates Speaker { get; set; } = null!;
    public Conference Conference { get; set; } = null!;
}
