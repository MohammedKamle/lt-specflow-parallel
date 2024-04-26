using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TechTalk.SpecRun;

namespace SpecflowLambdatest.Drivers
{
	public class BrowserSeleniumDriverFactory
	{
		private readonly ConfigurationDriver _configurationDriver;
		private readonly TestRunContext _testRunContext;

		public BrowserSeleniumDriverFactory(ConfigurationDriver configurationDriver, TestRunContext testRunContext)
		{
			_configurationDriver = configurationDriver;
			_testRunContext = testRunContext;
		}

		public IWebDriver GetForBrowser(int browserId)
		{
			DesiredCapabilities caps = new DesiredCapabilities();
			// Set common capabilities like "browserstack.local", project, name, session
			foreach (var tuple in _configurationDriver.CommonCapabilities)
			{
				if (tuple.Key.ToString() == "name")
				{
					caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString() + " " + browserId.ToString());
				}
				else
				{
					caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
				}
			}
			// Set session specific capability
			var specificCap = _configurationDriver.Capabilities.ToList<IConfigurationSection>()[browserId];
			foreach (var tuple in specificCap.GetChildren().AsEnumerable())
			{
				// Console.WriteLine("!!");
				// Console.WriteLine(tuple.Key.ToString());
				// Console.WriteLine("@@");
				// Console.WriteLine(tuple.Value);
				caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
			}

			// sets remote URL
			string username = Environment.GetEnvironmentVariable("LT_USERNAME");
			if (username == null || username == "")
			{
				username = _configurationDriver.Username;
			}
			string access_key = Environment.GetEnvironmentVariable("LT_ACCESS_KEY");
			if (access_key == null || access_key == "")
			{
				access_key = _configurationDriver.AccessKey;
			}
			string remoteUrl = "https://";
			if (username != null && access_key != null)
			{
				remoteUrl += username + ":" + access_key + "@";
			}
			remoteUrl += _configurationDriver.SeleniumBaseUrl + "/wd/hub";

			return new RemoteWebDriver(new Uri(remoteUrl), caps);
			// return null;
		}

		
	}
}
