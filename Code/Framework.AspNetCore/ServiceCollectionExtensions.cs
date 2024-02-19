using Framework.AspNetCore.MiddleWares;
using Framework.AspNetCore.Response;
using Framework.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigHttpContext(this IServiceCollection collection)
        {
            collection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            collection.TryAddSingleton<ICurrentUser, HttpContextUserResolverAdapter>();
        }
        public static void AddCqrsConvention(this MvcOptions options)
        {
            options.Conventions.Add(new CqrsConvention());
        }

        public static IMvcBuilder ConfigureApiBehaviorModelStateError(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var messages = c.ModelState.GetAllErrors();

                    var response = new ErrorResponseModel { Errors = messages.Select(msg => ExceptionDetails.Create(msg, 0, "")).ToList() };

                    return new BadRequestObjectResult(response);
                };
            });
            return mvcBuilder;
        }
    }
}
