using AngleSharp.Text;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace LambdaTest
{
    public class TodoMVC : TodoMVCBaseClass
    {
        [Theory]
        //[InlineData("Lit")]
        //[InlineData("React")]
        //[InlineData("React Redux")]
        //[InlineData("Vue.js")]
        //[InlineData("Preact")]
        //[InlineData("Backbone.js")]
        //[InlineData("Angular")]
        //[InlineData("Ember.js")]
        //[InlineData("KnockoutJS")]
        //[InlineData("Dojo")]
        //[InlineData("Knockback.js")]
        //[InlineData("CanJS")]
        [InlineData("Polymer")]
        //[InlineData("Mithril")]
        //[InlineData("Marionette.js")]
        public void VerifyItemsAddedInTodoList(string appName)
        {
            GotoTodoMVCSite();
            GoToApp(appName);
            AddTodoItem("Wash Car");
            AddTodoItem("Plan daily dose");
            VerifyItemAdded("Wash Car");
            VerifyNumberOfItems(2);
            CheckTodoItem("Wash Car");
            VerifyNumberOfItems(1);
        }

    }
}
