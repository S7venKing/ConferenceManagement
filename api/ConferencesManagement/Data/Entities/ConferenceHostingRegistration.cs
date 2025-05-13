using System;
using System.Collections.Generic;

namespace ConferencesManagementDAO.Data.Entities;

public partial class ConferenceHostingRegistration
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Location { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public string? Description { get; set; }

    public int RegisterId { get; set; }

    public bool NeedRegistrationFee { get; set; } = false;
    public decimal RegistrationFee { get; set; } = 0;

    public string Status { get; set; } = "Pending";

    public virtual Delegates Register { get; set; } = null!;
}
