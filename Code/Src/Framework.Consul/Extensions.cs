using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Winton.Extensions.Configuration.Consul;

namespace Framework.Consul
{
	public static class Extensions
	{
		public static IConfigurationBuilder AddConsulKeyValue(this IConfigurationBuilder config, string configurationFilePath)
		{
			if (string.IsNullOrWhiteSpace(configurationFilePath))
				throw new ArgumentNullException(nameof(configurationFilePath));

			if (!File.Exists(configurationFilePath))
				throw new ArgumentException("Unable to find consul configuration file", nameof(configurationFilePath));

			var consulConfiguration = JsonConvert.DeserializeObject<ConsulConfiguration>(File.ReadAllText(configurationFilePath));

			foreach (var source in consulConfiguration.SourceList)
			{
				config.AddConsul(source.Key, options =>
				{
					options.ConsulConfigurationOptions = configuration =>
					{
						configuration.Address = new Uri(source.Address);
						configuration.Datacenter = source.DataCenter;
					};

					options.OnLoadException = context =>
					{
						Console.WriteLine((string)context.Exception?.Message);
						context.Ignore = false;
					};

					options.Optional = true;
					options.ReloadOnChange = true;
				});
			}

			return config;
		}
	}
}
