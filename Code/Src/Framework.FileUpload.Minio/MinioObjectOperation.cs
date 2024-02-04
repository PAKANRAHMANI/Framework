using Minio;
using Minio.DataModel.Args;

namespace Framework.FileUpload.Minio;

public class MinioObjectOperation : IMinioObjectOperation
{
    private readonly IMinioClient _minioClient;

    public MinioObjectOperation(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task SaveObject(string bucketName, string objectName, string contentType, byte[] file, CancellationToken cancellationToken)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);

            if (await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false))
            {
                using var fileMemoryStream = new MemoryStream(file);

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(fileMemoryStream)
                    .WithObjectSize(fileMemoryStream.Length)
                    .WithContentType(contentType);

                await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<byte[]> GetObject(string bucketName, string objectName, string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var stream = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream(x => x.CopyTo(stream));

            await _minioClient.GetObjectAsync(args, cancellationToken).ConfigureAwait(false);

            return stream.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}