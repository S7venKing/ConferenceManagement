using System.ComponentModel.DataAnnotations;


namespace ConferencesManagementDAO.Data.Entities
{
    public class SpeakerConference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SpeakerId { get; set; }

        [Required]
        public int ConferenceId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending"; // "pending", "approved", "rejected"

        // Navigation properties
        public virtual Delegates? Speaker { get; set; }
        public virtual Conference? Conference { get; set; }
    }
}
