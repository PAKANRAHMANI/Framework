using Framework.AspNetCore.Models;
using Microsoft.Extensions.Options;

namespace Framework.AspNetCore
{
    public class ImagesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<FileStorageConfig> _storageConfig;
        public ImagesMiddleware(RequestDelegate next, IOptions<FileStorageConfig> storageConfig)
        {
            _next = next;
            _storageConfig = storageConfig;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!IsImageRequested(context))
            {
                await _next(context);
                return;
            }

            var imagePath = GetRequestedImagePath(context);
            var fullImagePath = Path.Combine(_storageConfig.Value.RootPath, imagePath);

            if (!File.Exists(fullImagePath))
            {
                context.Response.StatusCode = 404;
                return;
            }

            if (IsThumbnailRequested(context))
            {
                var size = CreateSizeFromQueryString(context);
                fullImagePath = ThumbnailHelper.GenerateThumbnail(fullImagePath, size);
            }

            await using var fs = new FileStream(fullImagePath, FileMode.Open);
            await fs.CopyToAsync(context.Response.Body);
        }

        private Dimension CreateSizeFromQueryString(HttpContext context)
        {
            return new Dimension()
            {
                Height = context.Request.Query.SafeGetValue<int>("height"),
                Width = context.Request.Query.SafeGetValue<int>("width"),
            };
        }

        private bool IsThumbnailRequested(HttpContext context)
        {
            return context.Request.Query.Count > 0;
        }

        private static bool IsImageRequested(HttpContext context)
        {
            return context.Request.Method.Equals("get", StringComparison.OrdinalIgnoreCase) &&
                   context.Request.Path.StartsWithSegments("/images", StringComparison.OrdinalIgnoreCase);
        }
        private string GetRequestedImagePath(HttpContext context)
        {
            var path = context.Request.Path.Value;
            var indexOfImages = path.IndexOf("/images", StringComparison.OrdinalIgnoreCase);
            return path.Substring(indexOfImages, path.Length - indexOfImages)
                        .Replace("/images/", "");
        }

    }
}
