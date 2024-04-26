using System;
using System.IO;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using SpecflowLambdatest.Drivers;

namespace SpecflowLambdatest.Support
{
	[Binding]
	public class Screenshots
	{
		private readonly WebDriver _webDriver;

		public Screenshots(WebDriver webDriver)
		{
			_webDriver = webDriver;
		}

		[AfterStep]
		public void MakeScreenshotAfterStep()
		{
			if (!(_webDriver.Current is ITakesScreenshot takesScreenshot))
			{
				return;
			}

			var screenshot = takesScreenshot.GetScreenshot();

			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
			string fileName = $"{fileNameWithoutExtension}.png";
			string tempFileName = Path.Combine(Directory.GetCurrentDirectory(), fileName);

			screenshot.SaveAsFile(tempFileName, ScreenshotImageFormat.Png);

			Console.WriteLine($"SCREENSHOT[ file:///{tempFileName} ]SCREENSHOT");
		}
	}
}
