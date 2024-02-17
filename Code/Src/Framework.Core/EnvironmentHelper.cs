using System;
using System.Linq;

namespace Framework.Core
{
    public class EnvironmentHelper
    {
        public static string GetEnvironment()
        {
            var environment =
                Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                Environments.Development;

            return GetValidEnvironmentString(environment);
        }

        static string GetValidEnvironmentString(string environment)
        {
            var environments = new[] { Environments.Development, Environments.Staging, Environments.Production, Environments.Acceptance, Environments.Oms, Environments.Hotfix };

            return environments.First(env => string.Equals(environment.ToUpper(), env.ToUpper(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
