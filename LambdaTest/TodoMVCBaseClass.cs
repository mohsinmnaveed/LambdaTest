using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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

            new DriverManager().SetUpDriver(new FirefoxConfig(), VersionResolveStrategy.Latest);
            driver = new FirefoxDriver();
            
            //ChromeOptions options = new ChromeOptions();
            ////Resolved: HTTP request to the remote WebDriver server for URL http://localhost:52847/….. timed out after 60 seconds
            //options.AddArgument("--start-maximized"); // open Browser in maximized mode
            ////options.AddArguments("disable-infobars"); // disabling infobars
            ////options.AddArguments("--disable-extensions"); // disabling extensions
            ////options.AddArguments("--disable-gpu"); // applicable to windows os only
            ////options.AddArguments("--disable-dev-shm-usage"); // overcome limited resource problems
            //options.AddArgument("--no-sandbox"); // Bypass OS security model
            //new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            //driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TIME_TO_WAIT_FOR_ELEMENT));
            action = new Actions(driver);
        }

        public void Dispose()
        {
            driver.Quit();
        }

        public void GotoTodoMVCSite()
        {
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
