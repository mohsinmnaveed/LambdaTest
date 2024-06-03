using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace LambdaTest
{

    /// <summary>
    /// Testing Lambdatest sample app : https://lambdatest.github.io/sample-todo-app/
    /// </summary>
    [TestCaseOrderer("LambdaTest.PriorityOrderer", "LambdaTest")]
    public class LambdaTestGithub : LambdaBaseClass
    {

        /// <summary>
        /// Verify that title is correct
        /// </summary>
        [RetryFact(MaxRetries = 5), TestPriority(1)]
        [Trait("Category", "Sanity")]
        public void TitleVerification()
        {
            GotoLambdaTestSite();
            Assert.Equal("Sample page - lambdatest.com", driver.Title);
        }


        //Verify that date is added with configured Culture
        [Fact, TestPriority(0)]
        [UseCulture("fi_FI")]
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


        //Verify the app name (Skippable)
        [Fact(Skip = "Not important"), TestPriority(2)]
        [Trait("Category", "UI")]
        public void CheckAppName()
        {
            GotoLambdaTestSite();

            Assert.Equal("LambdaTest Sample App", GetAppName().Text);
        }

        //Verify the app name (Skippable with NuGet Package)
        [SkippableFact(typeof(NoSuchElementException)), TestPriority(3)]
        [Trait("Category", "UI")]
        public void CheckAppNameSkippable()
        {
            GotoLambdaTestSite();

            Assert.Equal("LambdaTest Sample App", GetWrongAppName().Text);
        }

    }
}