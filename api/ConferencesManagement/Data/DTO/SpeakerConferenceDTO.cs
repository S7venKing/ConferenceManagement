namespace ConferencesManagementAPI.Data.DTO
{
    public class SpeakerConferenceDTO
    {
        public int SpeakerId { get; set; }
        public int ConferenceId { get; set; }

        public string? SpeakerName { get; set; }
        public string? ConferenceName { get; set; }
        public string Status { get; set; } = "pending"; // pending, approved, rejected
    }

    public class ApproveSpeakerDTO
    {
        public int SpeakerConferenceId { get; set; }
        public string Status { get; set; } = "approved"; // approved, rejected
    }

    public class SpeakerFileUploadDTO
    {
        public int SpeakerConferenceId { get; set; }
        public IFormFile File { get; set; } = null!;
    }


    public class SpeakerConferenceFileDTO
    {
        public int Id { get; set; }
        public string? SpeakerName { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
    }
}
