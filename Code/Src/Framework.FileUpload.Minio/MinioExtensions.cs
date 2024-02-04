using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.DataModel.Result;
using Minio.Handlers;

namespace Framework.FileUpload.Minio
{
    public static class MinioExtensions
    {
        public static IServiceCollection RegisterMinio(this IServiceCollection services, Action<MinioConfiguration> config)
        {
            var minioConfiguration = new MinioConfiguration();

            config.Invoke(minioConfiguration);

            services
                .AddMinio(configureClient => configureClient
                .WithEndpoint(minioConfiguration.Endpoint)
                .WithCredentials(minioConfiguration.AccessKey, minioConfiguration.SecretKey)
                .WithSSL(minioConfiguration.UseSSL)
                .WithTimeout(minioConfiguration.TimeOut));

            services.AddSingleton<IMinioObjectOperation, MinioObjectOperation>();

            return services;
        }
    }
}
