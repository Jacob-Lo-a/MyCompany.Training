using Microsoft.Extensions.Options;
using Renci.SshNet;
using Training.Core;
using Training.Core.interfaces;

namespace Training.API.Services
{
    public class SftpService : ISftpService
    {
        private readonly SftpSettings _settings;
        
        public SftpService(IOptions<SftpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task UploadReportAsync(byte[] fileData, string remoteFileName)
        {
            using var client = new SftpClient(
                _settings.Host,
                _settings.Port,
                _settings.Username,
                _settings.Password
            );

            client.Connect();

            using var stream = new MemoryStream(fileData);

            var remoteFullPath = $"{_settings.RemotePath}{remoteFileName}";

            client.UploadFile(stream, remoteFullPath, true); // 可覆蓋檔案

            client.Disconnect();

            await Task.CompletedTask;
        }

        public async Task UploadFileAsync(IFormFile file)
        {
            
            using var client = new SftpClient(
                _settings.Host,
                _settings.Port,
                _settings.Username,
                _settings.Password
            );

            client.Connect();

            using var stream = file.OpenReadStream();

            var remoteFullPath = $"{_settings.RemotePath}{file.FileName}";

            // 上傳檔案，可覆蓋
            client.UploadFile(stream, remoteFullPath, true);

            client.Disconnect();

            await Task.CompletedTask;
        }
        
    }
}