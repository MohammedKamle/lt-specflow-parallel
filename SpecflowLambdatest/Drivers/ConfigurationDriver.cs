using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace SpecflowLambdatest.Drivers
{
	public class ConfigurationDriver
	{
		private const string SeleniumBaseUrlConfigFieldName = "host";
		private readonly Lazy<IConfiguration> _configurationLazy;

		public ConfigurationDriver()
		{
			_configurationLazy = new Lazy<IConfiguration>(GetConfiguration);
		}

		public IConfiguration Configuration => _configurationLazy.Value;

		public string SeleniumBaseUrl => Configuration[SeleniumBaseUrlConfigFieldName];

		public string Username => Configuration["username"];
		public string AccessKey => Configuration["access_key"];

		public IEnumerable<IConfigurationSection> CommonCapabilities => Configuration.GetSection("commonCapabilities").GetChildren();

		public IEnumerable<IConfigurationSection> Capabilities => Configuration.GetSection("capabilities").GetChildren();

		private IConfiguration GetConfiguration()
		{
			var configurationBuilder = new ConfigurationBuilder();

			string directoryName = Path.GetDirectoryName(typeof(ConfigurationDriver).Assembly.Location);
			configurationBuilder.AddJsonFile(Path.Combine(directoryName, @"lambdatest_config.json"));

			return configurationBuilder.Build();
		}
	}
}
