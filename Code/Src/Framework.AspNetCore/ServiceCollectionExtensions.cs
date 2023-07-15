using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.AspNetCore.MiddleWares;
using Framework.AspNetCore.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Framework.Core;

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

                    var response = messages.Select(msg => ExceptionDetails.Create(msg, 0, ""));

                    return new BadRequestObjectResult(response);
                };
            });
            return mvcBuilder;
        }
    }
}
