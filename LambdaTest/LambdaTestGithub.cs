using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LambdaTest
{

    /// <summary>
    /// Testing Lambdatest sample app : https://lambdatest.github.io/sample-todo-app/
    /// </summary>
    public class LambdaTestGithub : BaseClass
    {

        /// <summary>
        /// Verify that title is correct
        /// </summary>
        [Fact]
        [Trait("Priority","1")]
        [Trait("Category", "Sanity")]
        public void TitleVerification()
        {
            GotoLambdaTestSite();
            Assert.Equal("Sample page - lambdatest.com", driver.Title);
        }

        [Fact]
        [Trait("Priority", "2")]
        [Trait("Category", "Functional")]
        public void AddDateToList() {
            GotoLambdaTestSite();
            IWebElement listItem;

            DateTime bDate = new DateTime(1994, 05, 29);
            string bDay = bDate.ToString("d");

            TodoTextInput().SendKeys(bDay);

            AddBtn().Click();

            listItem = TodoList().Last();
            listItem.FindElement(By.TagName("input")).Click();

            string check = listItem.FindElement(By.TagName("span")).GetAttribute("class");
            Assert.Equal("done-true", check);
        }
    }
}