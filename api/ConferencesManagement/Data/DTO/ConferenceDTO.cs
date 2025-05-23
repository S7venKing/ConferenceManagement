﻿namespace ConferencesManagementAPI.Data.DTO
{
    public class AddConferenceRequestDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = null!;

        public bool NeedRegistrationFee { get; set; } = false;
        public decimal RegistrationFee { get; set; } = 0;

        public int? HostBy { get; set; }
    }

    public class UpdateConferenceRequestDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool NeedRegistrationFee { get; set; } = false;
        public decimal RegistrationFee { get; set; } = 0;
        public string Location { get; set; } = null!;
    }

    public class ConferenceResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public decimal RegistrationFee { get; set; } = 0;

        public int? HostById { get; set; }

        public string HostByName { get; set; } = "";
        public bool HostByMe { get; set; }
    }
}
