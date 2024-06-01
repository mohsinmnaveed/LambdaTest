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
        [RetryFact(MaxRetries = 5)]
        [Trait("Priority","1")]
        [Trait("Category", "Sanity")]
        public void TitleVerification()
        {
            GotoLambdaTestSite();
            Assert.Equal("Sample page - lambdatest.com", driver.Title);
        }


        //Verify that date is added with configured Culture
        [Fact]
        [UseCulture("fi_FI")]
        [Trait("Priority", "2")]
        [Trait("Category", "Functional")]
        public void AddDateToList() {
            GotoLambdaTestSite();

            DateTime bDate = new DateTime(1994, 05, 29);
            string bDay = bDate.ToString("d");

            //Enter Date
            TodoTextInput().SendKeys(bDay);

            //Click Add button
            AddBtn().Click();

            //Click/Check the newly added date
            TodoList().Last().FindElement(By.TagName("input")).Click();

            //Verify that new date is checked
            Assert.Equal("29/5/1994", TodoList().Last().Text);
        }
    }
}