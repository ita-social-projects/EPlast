using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace EPlast.AutomatedTest
{
    public class ExampleTest : IDisposable
    {
        private readonly IWebDriver _driver;
        public ExampleTest()
        {
            _driver = new ChromeDriver();
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

    }
}
