using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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
    public class TodoMVCBaseClass : IDisposable
    {
        public const int TIME_TO_WAIT_FOR_ELEMENT = 5;
        public IWebDriver driver;
        WebDriverWait wait;
        Actions action;

        public TodoMVCBaseClass() 
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TIME_TO_WAIT_FOR_ELEMENT));
            driver.Manage().Window.Maximize();
            action = new Actions(driver);
        }

        public void Dispose()
        {
            driver.Quit();
        }

        public void GotoTotoMVCSite() {
            driver.Navigate().GoToUrl("https://todomvc.com/");
        }

        public void GoToApp(string v)
        {
            GetAppLink(v).Click();
        }

        private IWebElement GetAppLink(string v)
        {
            return wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//span[text()='{v}']")));
        }

        public void AddTodoItem(string v)
        {
            action.Click(TodoInput()).SendKeys(v).SendKeys(Keys.Enter).Perform();
        }

        public void VerifyItemAdded(string v)
        {
            string todoItem = "";
            try
            {
                todoItem = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//label[text()='{v}']"))).Text;
            }
            catch (Exception NoSuchElementException) 
            {
                todoItem = GetShadowedTodoListItem(v);
            }
            Assert.Equal(v, todoItem);
        }

        #region Webelement

        public IWebElement TodoInput()
        {
            try
            {
                return wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@placeholder='What needs to be done?']")));
            }
            catch (Exception NoSuchElementException)
            {
                return GetShadowedTodoFormRoot().FindElement(By.CssSelector("input[placeholder='What needs to be done?']"));
            }
            
        }

        public ISearchContext GetShadowedTodoAppRoot()
        {
            return driver.FindElement(By.CssSelector("todo-app")).GetShadowRoot();
        }

        public ISearchContext GetShadowedTodoFormRoot() 
        {
            
            return GetShadowedTodoAppRoot().FindElement(By.CssSelector("todo-form")).GetShadowRoot();
        }

        public ISearchContext GetShadowedTodoListRoot() 
        {
            return GetShadowedTodoAppRoot().FindElement(By.CssSelector("todo-list")).GetShadowRoot();
        }

        public String GetShadowedTodoListItem(String v)
        {
            String todoItem = "";
            List<IWebElement> listItems = GetShadowedTodoListRoot().FindElements(By.CssSelector("todo-item")).ToList();
            IWebElement i = GetShadowedTodoListRoot().FindElement(By.CssSelector("todo-item"));
            foreach (IWebElement item in listItems)
                if (item.GetShadowRoot().FindElement(By.CssSelector("label")).Text.Equals(v))
                    todoItem = v;

            return todoItem;
        }


        #endregion
    }
}
