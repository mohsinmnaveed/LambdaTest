using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace LambdaTest
{
    public class BaseClass : IDisposable
    {
        protected IWebDriver driver;

        protected BaseClass() {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        public void GotoLambdaTestSite() {
            driver.Navigate().GoToUrl("https://lambdatest.github.io/sample-todo-app/");
        }

        #region WebElements
        public IWebElement TodoTextInput() {
            return driver.FindElement(By.Id("sampletodotext"));
        }

        public IWebElement AddBtn() {
            return driver.FindElement(By.Id("addbutton"));
        }

        public List<IWebElement> TodoList() { 
            return driver.FindElement(By.ClassName("list-unstyled")).FindElements(By.TagName("li")).ToList();
        }

        public IWebElement GetAppName() {
            return driver.FindElement(By.TagName("h2"));
        }

        public IWebElement GetWrongAppName()
        {
            return driver.FindElement(By.TagName("h5"));
        }

        #endregion
        public void Dispose()
        {
            driver.Quit();
        }
    }
}
