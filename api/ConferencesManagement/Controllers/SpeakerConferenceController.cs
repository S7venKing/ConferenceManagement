using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementAPI.Services;
using ConferencesManagementAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConferencesManagementAPI.Controllers
{
    [Route("api/speaker-conference")]
    [ApiController]
    public class SpeakerConferenceController : ControllerBase
    {
        private readonly SpeakerConferenceService _service;

        private readonly SpeakerConferenceFileService _fileService;

        public SpeakerConferenceController(SpeakerConferenceService service, SpeakerConferenceFileService fileService)
        {
            _service = service;
            _fileService = fileService;
        }

        /// <summary>
        /// Đăng ký làm diễn giả của hội thảo
        /// </summary>
        [HttpPost("register")]
        [AuthorizeUser]
        public async Task<IActionResult> RegisterSpeaker([FromBody] SpeakerConferenceDTO dto)
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
            var response = await _service.AddAsync(dto);
            return response.isSuccess ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Xóa diễn giả khỏi hội thảo
        /// </summary>
        [HttpDelete("remove/{id}")]
        [AuthorizeAdmin]
        public async Task<IActionResult> RemoveSpeaker(int id)
        {
            var response = await _service.DeleteAsync(id);
            return response.isSuccess ? Ok(response) : NotFound(response);
        }

        /// <summary>
        /// Xem danh sách diễn giả của hội thảo
        /// </summary>
        [HttpGet("conference/{conferenceId}")]
        public async Task<IActionResult> GetSpeakersByConference(int conferenceId)
        {
            var response = await _service.GetSpeakersByConference(conferenceId);
            return response.isSuccess ? Ok(response) : NotFound(response);
        }

        /// <summary>
        /// Duyệt đơn đăng ký làm diễn giả của hội thảo
        /// </summary>
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveSpeaker([FromBody] ApproveSpeakerDTO dto)
        {
            var response = await _service.ApproveSpeakerAsync(dto.SpeakerConferenceId, dto.Status);
            return response.isSuccess ? Ok(response) : NotFound(response);
        }



        /// <summary>
        /// Hủy đơn đăng ký làm diễn giả của hội thảo
        /// </summary>
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelSpeakerRegistration(int id)
        {
            var response = await _service.CancelRegistrationAsync(id);
            return response.isSuccess ? Ok(response) : NotFound(response);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] int conferenceId, [FromForm] IFormFile file)
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
            await _service.GetByConfenrenceIdAndDelegateId(userId.Value, conferenceId);
            var result = await _fileService.UploadFileAsync(userId.Value, conferenceId, file);
            return result.isSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-all-speaker-file")]
        public async Task<IActionResult> GetFilesByConference([FromQuery] int conferenceId)
        {
            var files = await _fileService.GetFilesByConferenceAsync(conferenceId);
            return Ok(files);
        }

        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            var (fileBytes, fileName) = await _fileService.DownloadFileAsync(fileId);
            if (fileBytes == null) return NotFound("File not found");

            return File(fileBytes, "application/pdf", fileName);
        }

        [HttpPost("remove/{fileId}")]
        public async Task<IActionResult> RemoveFile(int fileId)
        {
            await _fileService.RemoveFileAsync(fileId);

            return Ok();
        }

        [HttpGet("preview/{fileId}")]
        public async Task<IActionResult> PreviewFile(int fileId)
        {
            var (fileBytes, _) = await _fileService.DownloadFileAsync(fileId);
            if (fileBytes == null) return NotFound("File not found");

            return File(fileBytes, "application/pdf");
        }
    }
}
