namespace Framework.FileUpload.Minio;

public interface IMinioObjectOperation
{
    public Task SaveObject(string bucketName, string objectName, string contentType, byte[] file, CancellationToken cancellationToken);
    public Task<byte[]> GetObject(string bucketName, string objectName, string fileName, CancellationToken cancellationToken);
}