using System;
using System.Collections.Generic;

namespace ConferencesManagementDAO.Data.Entities;

public partial class Conference
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Location { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? Description { get; set; }

    public int? HostBy { get; set; }

    public bool NeedRegistrationFee { get; set; } = false;
    public decimal RegistrationFee { get; set; } = 0;

    public virtual ICollection<DelegateConferenceRole> DelegateConferenceRoles { get; set; } = new List<DelegateConferenceRole>();

    public virtual Delegates? HostByNavigation { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<SpeakerConference> SpeakerConferences { get; set; } = new List<SpeakerConference>();

}
