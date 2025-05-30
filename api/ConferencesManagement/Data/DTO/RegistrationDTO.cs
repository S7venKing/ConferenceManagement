﻿using ConferencesManagementAPI.Constants;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConferencesManagementAPI.Data.DTO
{
    public class RegistrationResponseDTO
    {
        public int Id { get; set; }
        public int DelegateId { get; set; }
        public int ConferenceId { get; set; }

        public string? DelegateName { get; set; }

        public string? DelegateEmail { get; set; }
        public string? ConferenceName { get; set; }
        public string? Status { get; set; }

        public bool NeedPayFee { get; set; }

        public bool IsPayFee { get; set; }

        public string ConferenceRole { get; set; } = "Khách mời";

        public int ConferenceRoleId { get; set; } = 3;

        public DateTime? RegisteredAt { get; set; }
    }

    public class AddRegistrationRequestDTO
    {
        public int DelegateId { get; set; }
        public int ConferenceId { get; set; }
        public int ConferenceRoleId { get; set; }
        public string Status { get; set; } = RegistrationStatusConstants.STATUS_Pending;
    }

    public class DelegateJoinConferenceRequestDTO
    {
        public int ConferenceId { get; set; }
        public int ConferenceRoleId { get; set; }
        public string Status { get; set; } = RegistrationStatusConstants.STATUS_Pending;
    }

    public class CheckingConferenceAndDelageRequestDTO
    {
        public int ConferenceId { get; set; }
        public int DelegateId { get; set; }
    }

    public class AdminAddDelegatesToConferenceRequestDTO
    {
        public int ConferenceId { get; set; }

        public int DelegateId { get; set; }

        public int ConferenceRoleId { get; set; }
        public string Status { get; set; } = RegistrationStatusConstants.STATUS_Pending;
    }

    public class UpdateRegistrationRequestDTO
    {
        public int RegistrationId { get; set; }
        public string Status { get; set; } = null!;
    }

}
