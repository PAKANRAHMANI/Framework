using System.Threading.Tasks;

namespace Framework.FileUpload
{
    public interface IPersistFile
    {
        Task<string> SaveOnDisk(string extension, byte[] file);
    }
}
