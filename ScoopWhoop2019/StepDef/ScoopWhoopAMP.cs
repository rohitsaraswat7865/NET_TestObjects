using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TestAgent;


namespace ScoopWhoopTestScript
{
    [Binding]
    public sealed class ScoopWhoopAMP
    {
        GoogleSearchPage googleSearchPage;

        public ScoopWhoopAMP()
        {
            this.googleSearchPage = new GoogleSearchPage();
        }


        [Given(@"Google home page loaded")]
        public void GivenGoogleHomePageLoaded()
        {
            try
            {
                Assert.IsTrue(this.googleSearchPage.GoogleHomePageLoaded());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        //[When(@"Enter (.*) in google search bar on google home page")]
        //public void WhenEnterInGoogleSearchBarOnGoogleHomePage(string scoopWhoop)
        //{
        //    try
        //    {
        //        Assert.IsTrue(this.googleSearchPage.EnterInGoogleSearchBarOnGoogleHomePage(scoopWhoop));
        //    }
        //    catch(Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }

        //}

        [When(@"Click search button on google home page")]
        public void WhenClickSearchButtonOnGoogleHomePage()
        {
            try
            {
                Assert.IsTrue(this.googleSearchPage.ClickSearchButtonOnGoogleHomePage());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        //[Then(@"Redirected to (.*) search results")]
        //public void ThenRedirectedToSearchResults(string scoopWhoop)
        //{
        //    try
        //    {
        //        Assert.IsTrue(this.googleSearchPage.RedirectedToSearchResults());
        //    }
        //    catch(Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }

        //}

        [Then(@"ScoopWhoop (.*) AMP elements are loaded")]
        public void ThenScoopWhoopScoopwhoopAMPElementsAreLoaded(string scoopWhoop)
        {
            try
            {
                Assert.IsTrue(this.googleSearchPage.ScoopWhoopAMPElementsAreLoaded(scoopWhoop));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }



        //[Then(@"ScoopWhoop AMP elements are loaded")]
        //public void ThenScoopWhoopAMPElementsAreLoaded()
        //{
        //    try
        //    {
        //        Assert.IsTrue(this.googleSearchPage.ScoopWhoopAMPElementsAreLoaded());
        //    }
        //    catch(Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }

        //}

        [Then(@"ScoopWhoop AMP elements are querable")]
        public void ThenScoopWhoopAMPElementsAreQuerable()
        {
            try
            {
                Assert.IsTrue(this.googleSearchPage.ScoopWhoopAMPElementsAreQuerable());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        [When(@"Enter (.*) in google search bar on google home page")]
        public void WhenEnterInGoogleSearchBarOnGoogleHomePage(string scoopWhoop)
        {
            try
            {
                Assert.IsTrue(this.googleSearchPage.EnterInGoogleSearchBarOnGoogleHomePage(scoopWhoop));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Then(@"Redirected to (.*) amp search results")]
        public void ThenRedirectedToSearchResults(string scoopWhoop)
        {
            try
            {
                Assert.IsTrue(this.googleSearchPage.RedirectedToSearchResults(scoopWhoop));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

    }
}
