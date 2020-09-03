using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TestAgent;

namespace ScoopWhoopTestScript
{
    public class GoogleSearchPage
    {
        TestObject testObject = (FeatureContext.Current["testObject"] as TestAgent.TestObject);
        Dictionary<string, By> locators = (FeatureContext.Current["locators"]) as Dictionary<string, By>;
        Dictionary<string, string> config = (FeatureContext.Current["configElements"]) as Dictionary<string, string>;
        TestAgent.Helper helper = new TestAgent.Helper();

        internal bool GoogleHomePageLoaded()
        {
            try
            {
                string subUrl = "google";
                if (helper.CheckIfPageIsLoaded(testObject, subUrl))
                {

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        internal bool EnterInGoogleSearchBarOnGoogleHomePage(string scoopWhoop)
        {
            try
            {

                IWebElement comboTextBox;
                if (helper.TryGetElement(testObject, locators["Home_ComboTextBox"], out comboTextBox))
                {
                    comboTextBox.Click();
                }
                else
                {
                    return false;
                }
                string subUrl = "google";
                if (helper.CheckIfPageIsLoaded(testObject, subUrl))
                {

                }
                else
                {
                    return false;
                }
                if (testObject.elementCache.TryGetValue(locators["Home_ComboTextBox"], out comboTextBox))
                {
                    comboTextBox.SendKeys(scoopWhoop);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        internal bool ClickSearchButtonOnGoogleHomePage()
        {
            try
            {
                IWebElement searchButton;
                if (helper.TryGetElement(testObject, locators["Home_SearchButton"], out searchButton))
                {
                    searchButton.Click();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        internal bool RedirectedToSearchResults(string scoopWhoop)
        {
            try
            {
                IWebElement searchResults;
                if (helper.TryGetElement(testObject, locators["Home_GTrayHeader"], out searchResults))
                {
                    if (!(searchResults.Text.ToLower().Contains(scoopWhoop)))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        internal bool ScoopWhoopAMPElementsAreLoaded(string scoopWhoop)
        {
            try
            {
                ReadOnlyCollection<IWebElement> ampElementList;
                if (helper.TryGetElementList(testObject, locators["Home_AMPLogo"], out ampElementList))
                {
                    if (!(ampElementList.Count > 0))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                ReadOnlyCollection<IWebElement> ampUriList;
                if (helper.TryGetElementList(testObject, locators["Home_AMPUris"], out ampUriList))
                {
                    foreach (IWebElement element in ampUriList)
                    {
                        string str = element.GetAttribute("data-amp");
                        if (!str.ToLower().Contains(scoopWhoop))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        internal bool ScoopWhoopAMPElementsAreQuerable()
        {
            try
            {
                ReadOnlyCollection<IWebElement> ampLinksList;
                if (helper.TryGetElementList(testObject, locators["Home_AMPHeadings"], out ampLinksList))
                {
                    foreach (IWebElement element in ampLinksList)
                    {
                        helper.CheckIfClickable(testObject, element);
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
    }
}
