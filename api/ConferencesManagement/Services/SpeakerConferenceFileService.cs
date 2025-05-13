using ConferencesManagementAPI.DAO.Repositories;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;

public class SpeakerConferenceFileService
{
    private readonly SpeakerConferenceFileRepositories _fileRepository;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<SpeakerConferenceFileService> _logger;

    public SpeakerConferenceFileService(
        SpeakerConferenceFileRepositories fileRepository,
        IWebHostEnvironment env,
        ILogger<SpeakerConferenceFileService> logger)
    {
        _fileRepository = fileRepository;
        _env = env;
        _logger = logger;
    }

    public async Task<GeneralResponseDTO> UploadFileAsync(int speakerId, int conferenceId, IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return new GeneralResponseDTO { isSuccess = false, Message = "File is empty" };

            if (!Directory.Exists(_env.WebRootPath))
            {
                Directory.CreateDirectory(_env.WebRootPath);
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var speakerFile = new SpeakerConferenceFile
            {
                SpeakerId = speakerId,
                ConferenceId = conferenceId,
                FileName = file.FileName,
                FilePath = fileName
            };

            await _fileRepository.AddFileAsync(speakerFile);
            await _fileRepository.SaveChangesAsync();

            return new GeneralResponseDTO { isSuccess = true, Message = "File uploaded successfully", data = speakerFile };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error uploading file: {ex.Message}");
            return new GeneralResponseDTO { isSuccess = false, Message = "Error uploading file" };
        }
    }

    public async Task<GeneralResponseDTO> RemoveFileAsync(int fileId)
    {
        try
        {
            // Tìm file trong database
            var file = await _fileRepository.GetFileByIdAsync(fileId);
            if (file == null)
                return new GeneralResponseDTO { isSuccess = false, Message = "File not found" };

            // Lấy đường dẫn file từ WebRootPath
            var filePath = Path.Combine(_env.WebRootPath, "uploads", file.FilePath);

            // Kiểm tra và xóa file vật lý nếu tồn tại
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Xóa file khỏi database
            _fileRepository.Remove(file);
            await _fileRepository.SaveChangesAsync();

            return new GeneralResponseDTO { isSuccess = true, Message = "File deleted successfully" };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting file: {ex.Message}");
            return new GeneralResponseDTO { isSuccess = false, Message = "Error deleting file" };
        }
    }


    public async Task<List<SpeakerConferenceFileDTO>> GetFilesByConferenceAsync(int conferenceId)
    {
        var files = await _fileRepository.GetFilesByConferenceIdAsync(conferenceId);
        return files.Select(f => new SpeakerConferenceFileDTO
        {
            Id = f.Id,
            SpeakerName = f.Speaker.FullName,
            FileName = f.FileName,
            FileUrl = $"/speaker-conference/download/{f.Id}"
        }).ToList();
    }

    public async Task<(byte[], string?)> DownloadFileAsync(int fileId)
    {
        var file = await _fileRepository.GetFileByIdAsync(fileId);
        if (file == null)
            return (null, null);

        var filePath = Path.Combine(_env.WebRootPath, "uploads", file.FilePath);
        if (!File.Exists(filePath))
            return (null, null);

        return (await File.ReadAllBytesAsync(filePath), file.FileName);
    }
}
