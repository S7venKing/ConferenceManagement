using ConferencesManagementAPI.DAO.Repositories;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.Extensions.Logging;

namespace ConferencesManagementAPI.Services
{
    public class SpeakerConferenceService
    {
        private readonly SpeakerConferenceRepositories _repository;
        private readonly ILogger<SpeakerConferenceService> _logger;

        public SpeakerConferenceService(SpeakerConferenceRepositories repository, ILogger<SpeakerConferenceService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GeneralResponseDTO> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Retrieved all speaker conferences successfully",
                    data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllAsync: {ex.Message}");
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred while fetching data" };
            }
        }

        public async Task<GeneralResponseDTO> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "SpeakerConference not found" };
                }
                return new GeneralResponseDTO { isSuccess = true, Message = "SpeakerConference retrieved", data = entity };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetByIdAsync: {ex.Message}");
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred while fetching data" };
            }
        }

        public async Task<GeneralResponseDTO> AddAsync(SpeakerConferenceDTO speakerConferenceDTO)
        {
            try
            {
                var newEntity = new SpeakerConference
                {
                    SpeakerId = speakerConferenceDTO.SpeakerId,
                    ConferenceId = speakerConferenceDTO.ConferenceId,
                    Status = speakerConferenceDTO.Status
                };

                await _repository.AddAsync(newEntity);
                await _repository.SaveChangesAsync();

                return new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "SpeakerConference created successfully",
                    data = newEntity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddAsync: {ex.Message}");
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred while adding data" };
            }
        }

        public async Task<GeneralResponseDTO> UpdateAsync(int id, SpeakerConferenceDTO speakerConferenceDTO)
        {
            try
            {
                var existingEntity = await _repository.GetByIdAsync(id);
                if (existingEntity == null)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "SpeakerConference not found" };
                }

                existingEntity.SpeakerId = speakerConferenceDTO.SpeakerId;
                existingEntity.ConferenceId = speakerConferenceDTO.ConferenceId;
                existingEntity.Status = speakerConferenceDTO.Status;

                await _repository.SaveChangesAsync();

                return new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "SpeakerConference updated successfully",
                    data = existingEntity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateAsync: {ex.Message}");
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred while updating data" };
            }
        }

        public async Task<GeneralResponseDTO> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "SpeakerConference not found" };
                }

                _repository.Remove(entity);
                await _repository.SaveChangesAsync();

                return new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "SpeakerConference deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteAsync: {ex.Message}");
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred while deleting data" };
            }
        }

        public async Task<GeneralResponseDTO> GetSpeakersByConference(int conferenceId)
        {
            try
            {
                var result = _repository.GetSpeakersByConference(conferenceId);
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred while deleting data", data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteAsync: {ex.Message}");
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred while deleting data" };
            }
        }
        /// <summary>
        /// Duyệt đơn đăng ký làm diễn giả
        /// </summary>
        public async Task<GeneralResponseDTO> ApproveSpeakerAsync(int id, string status)
        {
            try
            {
                var speakerConference = await _repository.GetByIdAsync(id);
                if (speakerConference == null)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "Speaker registration not found" };
                }

                // Kiểm tra trạng thái hợp lệ
                if (status.ToLower() != "approved" && status.ToLower() != "rejected")
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "Invalid status. Must be 'approved' or 'rejected'" };
                }

                // Cập nhật trạng thái
                speakerConference.Status = status.ToLower();
                await _repository.SaveChangesAsync();

                return new GeneralResponseDTO { isSuccess = true, Message = $"Speaker registration {status} successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ApproveSpeakerAsync: {ex.Message}");
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred while approving speaker" };
            }
        }

        /// <summary>
        /// Hủy đơn đăng ký của diễn giả
        /// </summary>
        public async Task<GeneralResponseDTO> CancelRegistrationAsync(int id)
        {
            try
            {
                var speakerConference = await _repository.GetByIdAsync(id);
                if (speakerConference == null)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "Speaker registration not found" };
                }

                // Xóa đăng ký
                _repository.Remove(speakerConference);
                await _repository.SaveChangesAsync();

                return new GeneralResponseDTO { isSuccess = true, Message = "Speaker registration canceled successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CancelRegistrationAsync: {ex.Message}");
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred while canceling registration" };
            }
        }

        public async Task<SpeakerConference?> GetByConfenrenceIdAndDelegateId(int confenrenceId, int delegateId)
        {
            return await _repository.FirstOrDefaultAsync(a => a.ConferenceId == confenrenceId && a.SpeakerId == delegateId);
        }

        public async Task SaveChange()
        {
            await _repository.SaveChangesAsync();
        }


    }
}


