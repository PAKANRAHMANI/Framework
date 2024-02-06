namespace Framework.FileUpload.Minio;

public interface IMinioObjectOperation
{
    public Task SaveObjectAsync(string bucketName, string objectName, string contentType, byte[] file, CancellationToken cancellationToken);
    public Task<byte[]> GetObjectAsync(string bucketName, string objectName, CancellationToken cancellationToken);
}