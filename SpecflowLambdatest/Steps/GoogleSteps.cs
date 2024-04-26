using TechTalk.SpecFlow;
using FluentAssertions;
using SpecflowLambdatest.Drivers;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using Xunit;
using System.Collections.Generic;

namespace SpecflowLambdatest.Steps
{
	[Binding]
	public class GoogleSteps
	{
		private  WebDriver _webDriver;
		private static bool passed = true;

		private static string message = "";

		public GoogleSteps(WebDriver driver)
		{
			_webDriver = driver;
		}

		[When(@"open Wikipedia app and click on search bar")]
		public void WhenOpenWikipediaAppAndClickOnSearchBar()
		{
			_webDriver.Current.FindElement(By.Id("search_container")).Click();
			
		}

		[Then(@"enter '(.*)'")]
		public void ThenEnter(string input)
		{
			System.Threading.Thread.Sleep(3000);
			_webDriver.Current.FindElement(By.Id("search_src_text")).SendKeys(input);
		}

        [Given(@"goto Google")]
        public void GivenINavigatedToGoogle()
        {
            _webDriver.Current.Navigate().GoToUrl("https://www.google.com/ncr");
        }

        [Then(@"title should be '(.*)'")]
        public void ThenTheResultShouldBeOnTheScreen(string expectedTitle)
        {
            string result = _webDriver.Wait.Until(d => d.Title.ToString());
            passed &= result.ToString().Equals(expectedTitle);
            if (!passed)
            {
                message = result.ToString() + " is not equal to " + expectedTitle;
            }
        }

        [AfterScenario]
		public void MarkTestAsPassOrFail()
		{
            if (passed)
            {
                ((IJavaScriptExecutor)_webDriver.Current).ExecuteScript("lambda-status=passed");
            }
            else
            {
                ((IJavaScriptExecutor)_webDriver.Current).ExecuteScript("lambda-status=passed");
            }
        }
    }
}
