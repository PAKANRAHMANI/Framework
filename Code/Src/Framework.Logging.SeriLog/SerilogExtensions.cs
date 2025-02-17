using Framework.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.Logging.SeriLog
{
    public static class SerilogExtensions
    {
        public static ILogger RegisterILogger(this IServiceCollection services, IConfiguration configuration)
        {
            var logger = SerilogConfigurator.Config(configuration);

            var adapter = new SeriLogAdapter(logger);

            services.TryAddSingleton<ILogger>(adapter);

            return adapter;
        }
    }
}
