using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
