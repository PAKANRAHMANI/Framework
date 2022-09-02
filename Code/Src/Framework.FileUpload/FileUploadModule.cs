using System.Net;
using FluentFTP;
using Framework.Config;


namespace Framework.FileUpload
{
    public class FileUploadModule : IFrameworkModule
    {
        private readonly FtpServerConfiguration _ftpServerConfiguration;

        public FileUploadModule(FtpServerConfiguration ftpServerConfiguration)
        {
            _ftpServerConfiguration = ftpServerConfiguration;
        }

        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped<IPersistFile, FtpPersist>();
            dependencyRegister.RegisterScoped<IFtpClient>(CreateFtpClient);
        }

        private FtpClient CreateFtpClient()
        {
            var client = new FtpClient($"{_ftpServerConfiguration.EndPoint}")
            {
                Credentials = new NetworkCredential(_ftpServerConfiguration.UserName, _ftpServerConfiguration.Password)
            };

            client.Connect();

            return client;
        }
    }
}