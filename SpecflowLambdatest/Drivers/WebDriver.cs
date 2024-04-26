using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SpecflowLambdatest.Drivers
{
	public class WebDriver : IDisposable
	{
		private readonly BrowserSeleniumDriverFactory _browserSeleniumDriverFactory;
		private readonly Lazy<IWebDriver> _currentWebDriverLazy;
		private readonly Lazy<WebDriverWait> _waitLazy;
		private readonly TimeSpan _waitDuration = TimeSpan.FromSeconds(10);
		private bool _isDisposed;

		public WebDriver(BrowserSeleniumDriverFactory browserSeleniumDriverFactory)
		{
			_browserSeleniumDriverFactory = browserSeleniumDriverFactory;
			_currentWebDriverLazy = new Lazy<IWebDriver>(GetWebDriver);
			_waitLazy = new Lazy<WebDriverWait>(GetWebDriverWait);
		}

		public IWebDriver Current => _currentWebDriverLazy.Value;

		public WebDriverWait Wait => _waitLazy.Value;

		private WebDriverWait GetWebDriverWait()
		{
			return new WebDriverWait(Current, _waitDuration);
		}

		private IWebDriver GetWebDriver()
		{
			string browserIndex = Environment.GetEnvironmentVariable("Test_Browser_Index");
			if (browserIndex == null)
			{
				browserIndex = "0";
			}
			int testBrowserId = Convert.ToInt32(browserIndex);
			//if (_currentLocal == null)
			//{
			//	_currentLocal = GetBrowserStackLocal();
			//}
			return _browserSeleniumDriverFactory.GetForBrowser(testBrowserId);
		}



		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}

			if (_currentWebDriverLazy.IsValueCreated)
			{
				Current.Quit();

			}

			_isDisposed = true;
		}
	}
}
