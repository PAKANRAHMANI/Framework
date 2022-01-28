using System.IO;
using System.Threading.Tasks;
using FluentFTP;
using Framework.FileUpload;
using Microsoft.Extensions.Options;

namespace Framework.FileUpload
{
    public class FtpPersist : IPersistFile
    {
        private readonly IOptions<FileStorageConfig> _storageConfiguration;
        private readonly IFtpClient _ftpClient;

        public FtpPersist(IOptions<FileStorageConfig> storageConfiguration, IFtpClient ftpClient)
        {
            _storageConfiguration = storageConfiguration;
            _ftpClient = ftpClient;
        }
        public async Task<string> SaveOnDisk(string extension, byte[] file)
        {
            var pathInfo = RandomFileName.GetRandomFileName();

            var directory = Path.Combine(_storageConfiguration.Value.HyperLinkIconPath, pathInfo.DirectoryName);

            await CreateFolderIfNotExist(directory);

            var fileName = $"{pathInfo.FileName}{extension}";

            var pathToSave = Path.Combine(_storageConfiguration.Value.HyperLinkIconPath, pathInfo.DirectoryName, fileName);

            await _ftpClient.UploadAsync(file, pathToSave);

            return Path.Combine(pathInfo.DirectoryName,fileName);
        }
        private async Task CreateFolderIfNotExist(string folderName)
        {
            var existFolder = await _ftpClient.DirectoryExistsAsync(folderName);
            if (existFolder == false)
                await _ftpClient.CreateDirectoryAsync(folderName);
        }
    }

}
