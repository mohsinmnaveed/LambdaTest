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
        public WebDriverWait wait;
        public Actions action;

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

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(180);
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

        public void AddTodoItem(string v)
        {
            action.Click(TodoInput()).SendKeys(v).SendKeys(Keys.Enter).Perform();
        }

        public void VerifyItemAdded(string v)
        {
            string todoItem;
            try
            {
                todoItem = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//label[text()='{v}']"))).Text;
            }
            catch (Exception ex)
            {
                todoItem = GetShadowedTodoListItem(v).Text;
            }
            Assert.Equal(v, todoItem);
        }

        public void CheckTodoItem(string v)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.XPath($"//label[text()='{v}']/preceding-sibling::input"))).Click();
            }
            catch (Exception ex)
            {
                GetShadowedTodoListItemCheckbox(v).Click();
            }
        }

        public void VerifyNumberOfItems(int v)
        {
            int actualItemsLeft = GetTodoItemLeftCount(GetTodoItemLeftElement());
            Assert.Equal(v, actualItemsLeft);
        }

        public int GetTodoItemLeftCount(IWebElement itemLeftElem)
        {
            string itemLeft = itemLeftElem.Text.ToString().Split(" ")[0];
            return Int32.Parse(itemLeft);
        }

        /// <summary>
        /// //// To find WebElements
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>

        #region Webelement

        private IWebElement GetAppLink(string v)
        {
            return wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//span[text()='{v}']")));
        }

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

        public ISearchContext GetShadowedTodoFooterRoot()
        {
            return GetShadowedTodoAppRoot().FindElement(By.CssSelector("todo-footer")).GetShadowRoot();
        }

        public List<IWebElement> GetShadowedTodoListItems()
        {
            return GetShadowedTodoListRoot().FindElements(By.CssSelector("todo-item")).ToList();
        }

        public IWebElement GetShadowedTodoListItem(String v)
        {
            IWebElement todoItem;
            List<IWebElement> listItems = GetShadowedTodoListItems();
            foreach (IWebElement item in listItems)
            {
                todoItem = item.GetShadowRoot().FindElement(By.CssSelector("label"));
                if (todoItem.Text.Equals(v))
                {
                    return todoItem;
                }
            }
            throw new NoSuchElementException("List item not found");
        }

        public IWebElement GetShadowedTodoListItemCheckbox(String v)
        {
            IWebElement todoItem;
            List<IWebElement> listItems = GetShadowedTodoListItems();
            foreach (IWebElement item in listItems)
            {
                todoItem = item.GetShadowRoot().FindElement(By.CssSelector("label"));
                if (todoItem.Text.Equals(v))
                {
                    return item.GetShadowRoot().FindElement(By.CssSelector("input"));
                }
            }
            throw new NoSuchElementException("List item not found");
        }

        public IWebElement GetTodoItemLeftElement()
        {
            try
            {
                return wait.Until(ExpectedConditions.ElementExists(By.CssSelector("span[class*='todo-count']")));
            }
            catch (Exception ex)
            {
                return GetShadowedTodoFooterRoot().FindElement(By.CssSelector("span[class='todo-count']"));
            }
        }


        #endregion
    }
}
