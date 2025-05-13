using Microsoft.AspNetCore.Mvc;
using ConferencesManagementService;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementAPI.Utils;
using ConferencesManagementAPI.Services;
using ConferencesManagementAPI.Constants;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly RegistrationService _registrationService;
        private readonly ConferenceService _conferenceService;

        private readonly SpeakerConferenceService _speakerConferenceService;

        private readonly DelegateConferenceRoleService _delegateConferenceRoleServices;


        public RegistrationsController(RegistrationService registrationService, ConferenceService conferenceService, RegistrationService registrationService1, SpeakerConferenceService speakerConferenceService, DelegateConferenceRoleService delegateConferenceRoleServices)
        {
            _registrationService = registrationService;
            _conferenceService = conferenceService;
            _speakerConferenceService = speakerConferenceService;
            _delegateConferenceRoleServices = delegateConferenceRoleServices;
        }

        // API: Đại biểu đăng ký hội thảo
        [HttpPost("delegate-register-conference")]
        [AuthorizeUser] // Chỉ đại biểu mới có thể đăng ký tham gia
        public async Task<IActionResult> RegisterForConference([FromBody] DelegateJoinConferenceRequestDTO registrationDTO)
        {
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);

                AddRegistrationRequestDTO addConferenceRequestDTO = new AddRegistrationRequestDTO
                {
                    ConferenceId = registrationDTO.ConferenceId,
                    ConferenceRoleId = registrationDTO.ConferenceRoleId,
                    Status = registrationDTO.Status,
                    DelegateId = userId.GetValueOrDefault(),
                };
                var delegateRegisterConferenceAsyncResponse = await _registrationService.DelegateRegisterConferenceAsync(addConferenceRequestDTO);
                if (!delegateRegisterConferenceAsyncResponse.isSuccess)
                {
                    return BadRequest(delegateRegisterConferenceAsyncResponse);
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Registered successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPost("delegates-check-registration-status")]
        [AuthorizeUser]
        public async Task<IActionResult> RegisterForConference([FromQuery] int conferenceId)
        {
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (userId == null)
                {
                    return Unauthorized(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid token"
                    });
                }

                var registrationResponses = await _registrationService.GetRegistrationByConfenreceIdAsync(conferenceId);
                var registration = registrationResponses.FirstOrDefault(r => r.DelegateId == userId);
                if (registration == null)
                {
                    registration = new RegistrationResponseDTO
                    {
                        Status = "Not registered"
                    };
                }
                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "",
                    data = registration
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        // API: Admin add đại biểu vào hội thảo
        [HttpPost("admin-add-delegates")]
        [AuthorizeUser]
        public async Task<IActionResult> AdminAddDelegatesToConferences([FromBody] AdminAddDelegatesToConferenceRequestDTO registrationDTO)
        {
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (userId == null)
                {
                    return Unauthorized(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid token"
                    });
                }
                var conference = await _conferenceService.GetConferenceByIdAsync(registrationDTO.ConferenceId);
                if (conference == null)
                {
                    return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = "Conference not esxited" });
                }

                if (conference.HostById != userId)
                {
                    return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = "You dont have permisstion because you are not host of this conference" });
                }
                AddRegistrationRequestDTO addConferenceRequestDTO = new AddRegistrationRequestDTO
                {
                    ConferenceId = registrationDTO.ConferenceId,
                    ConferenceRoleId = registrationDTO.ConferenceRoleId,
                    Status = registrationDTO.Status,
                    DelegateId = registrationDTO.DelegateId
                };
                var delegateRegisterConferenceAsyncResponse = await _registrationService.DelegateRegisterConferenceAsync(addConferenceRequestDTO);
                if (!delegateRegisterConferenceAsyncResponse.isSuccess)
                {
                    return BadRequest(delegateRegisterConferenceAsyncResponse);
                }

                if (registrationDTO.ConferenceRoleId == 2 && registrationDTO.Status == RegistrationStatusConstants.STATUS_Confirmed)
                {
                    await _speakerConferenceService.AddAsync(new SpeakerConferenceDTO
                    {
                        ConferenceId = registrationDTO.ConferenceId,
                        SpeakerId = registrationDTO.DelegateId,
                        Status = "approved"
                    });
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Registered successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        // API: Xem danh sách đăng ký hội thảo
        [HttpGet("get-by-id")]
        //[AuthorizeAdmin]
        public async Task<IActionResult> GetRegistrations([FromQuery] int conferenceId)
        {
            try
            {
                var registrations = await _registrationService.GetRegistrationByConfenreceIdAsync(conferenceId);
                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    data = registrations,
                    Message = "Get registration successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPost("unregistered-registration")]
        [AuthorizeUser]
        public async Task<IActionResult> UpdateRegistrationStatus([FromQuery] int conferenceId)
        {
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (userId == null)
                {
                    return Unauthorized(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid token"
                    });
                }

                var registrationResponses = await _registrationService.GetAllRegistrationsAsync();
                var registration = registrationResponses.FirstOrDefault(r => r.ConferenceId == conferenceId && r.DelegateId == userId);
                if (registration == null)
                {
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Delegate not registered for this conference"
                    });
                }
                var response = await _registrationService.DeleteRegistrationAsync(registration.Id);
                await _delegateConferenceRoleServices.DeleteRoleAsync(registration.DelegateId, registration.ConferenceId);
                if (!response)
                {
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Delegate had not registered for this conference"
                    });
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Unregistered successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AuthorizeUser]
        [HttpPost("payment-status/{conferenceId}")]
        public async Task<IActionResult> CheckPaymentStatus(int conferenceId)
        {
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (userId == null)
                {
                    return Unauthorized(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid token"
                    });
                }

                var registrationResponses = await _registrationService.GetAllRegistrationsAsync();
                var registration = registrationResponses.FirstOrDefault(r => r.ConferenceId == conferenceId && r.DelegateId == userId);
                if (registration == null)
                {
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Delegate not registered for this conference"
                    });
                }
                var paymentStatus = await _registrationService.GetRegistrationPaymentStatusByIdAsync(registration.Id);
                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Success",
                    data = paymentStatus
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [AuthorizeUser]
        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateRegistrationStatus([FromBody] UpdateRegistrationRequestDTO updateRegistrationRequestDTO)
        {
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (userId == null)
                {
                    return Unauthorized(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid token"
                    });
                }
                var registration = await _registrationService.GetRegistrationByIdAsync(updateRegistrationRequestDTO.RegistrationId);
                if (registration == null)
                {
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Registration not found"
                    });
                }
                var confenrence = await _conferenceService.GetConferenceByIdAsync(registration.ConferenceId);

                if (confenrence == null)
                {
                    return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = "Conference not esxited" });
                }

                if (confenrence.HostById != userId)
                {
                    return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = "You dont have permisstion because you are not host of this conference" });
                }

                var paymentStatus = _registrationService.GetRegistrationPaymentStatusByIdAsync(registration.Id);
                if (paymentStatus != null)
                {
                    new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Người tham dự đã nộp phí không thể thay đổi trạng thái"
                    };
                }

                var response = await _registrationService.UpdateRegistrationAsync(updateRegistrationRequestDTO.RegistrationId,
                                                                                    updateRegistrationRequestDTO.Status);


                if (!response.isSuccess)
                {
                    return BadRequest(response);
                }

                var role = await _delegateConferenceRoleServices.GetRoleByIdAsync(registration.DelegateId, registration.ConferenceId);


                if (
                    role != null && role.RoleId == 2)
                {
                    var speakerConference = await _speakerConferenceService.GetByConfenrenceIdAndDelegateId(registration.ConferenceId, registration.DelegateId);
                    if (speakerConference == null)
                    {
                        await _speakerConferenceService.AddAsync(new SpeakerConferenceDTO
                        {
                            ConferenceId = confenrence.Id,
                            SpeakerId = registration.DelegateId,
                            Status = updateRegistrationRequestDTO.Status
                        });
                    }
                    else
                    {
                        speakerConference.Status = updateRegistrationRequestDTO.Status;
                        await _speakerConferenceService.SaveChange();
                    }


                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
