namespace PublicServiceRegistry.OrafinUpload
{
    using System.IO;
    using System.Net;
    using System.Text;
    using FluentFTP;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public interface IFtpConnection
    {
        void UploadFile(FileName fileName, string content);
        void UploadFile(FileName fileName, byte[] data);
    }

    public class FtpConnection : IFtpConnection
    {
        private readonly ILogger<UploadService> _logger;
        private readonly FtpConfiguration _ftpConfiguration;

        public FtpConnection(
            ILogger<UploadService> logger,
            IOptions<FtpConfiguration> ftpConfiguration)
        {
            _logger = logger;
            _ftpConfiguration = ftpConfiguration.Value;
        }

        public void UploadFile(FileName fileName, string content)
            => UploadFile(fileName, Encoding.UTF8.GetBytes(content));

        public void UploadFile(FileName fileName, byte[] data)
        {
            if (fileName == null)
            {
                _logger.LogWarning("File name not set, upload to Orafin FTP server aborted.");
                return;
            }

#if DEBUG // TODO: remove debug output
            _logger.LogInformation("File: {fileName}", fileName);
            _logger.LogInformation("================================");
            _logger.LogInformation(Encoding.UTF8.GetString(data));
            _logger.LogInformation("================================");
#endif

            if (!_ftpConfiguration.IsSet)
            {
                _logger.LogWarning("FTP configuration not set, upload to Orafin FTP server aborted.");
                return;
            }

            _logger.LogInformation("Uploading {Subsidies} to Orafin FTP server.", fileName);

            var path = Path.Combine(_ftpConfiguration.RemotePath, fileName);
            var login = new NetworkCredential(_ftpConfiguration.User, _ftpConfiguration.Password);

            using (var ftp = new FtpClient(_ftpConfiguration.Host, login))
            {
                if (ftp.FileExists(path))
                    _logger.LogWarning("Overwriting {Subsidies} on the FTP server at {OrafinUploadPath}",
                        fileName,
                        path);

                ftp.RetryAttempts = 3;
                ftp.Upload(data, path);
            }

            _logger.LogInformation("Uploaded {Subsidies} to Orafin FTP server.", fileName);
        }
    }
}
