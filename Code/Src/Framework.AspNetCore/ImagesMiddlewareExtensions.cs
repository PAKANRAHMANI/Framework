namespace Framework.AspNetCore
{
    public static class ImagesMiddlewareExtensions
    {
        public static void UseImages(this IApplicationBuilder app)
        {
            app.UseMiddleware<ImagesMiddleware>();
        }
    }
}
