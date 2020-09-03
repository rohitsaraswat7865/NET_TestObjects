#region Notes

//https://www.maketecheasier.com/useful-chrome-command-line-switches/

#endregion
namespace TestAgent
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium.Chrome;
    using System.Collections.ObjectModel;
    using System.IO;

    public enum BrowserOptions
    {
        Chrome,
        Firefox
    }

    public enum AlignmentOptions
    {
        Desktop,
        MobileSite
    }
    public class TestObject : IDisposable
    {
        public IWebDriver driver { get; set; }
        public Dictionary<By, IWebElement> elementCache { get; set; }
        private Dictionary<int, string> dataCache;

        private TestObject(BrowserOptions browserOptions, AlignmentOptions alignOptions)
        {
            #region Init
            this.elementCache = new Dictionary<By, IWebElement>();
            this.dataCache = new Dictionary<int, string>();
            #endregion
            AppDomain domain = AppDomain.CurrentDomain;
            string path = domain.BaseDirectory;

            if (browserOptions.Equals(BrowserOptions.Chrome) && alignOptions.Equals(AlignmentOptions.Desktop))
            {
                ChromeOptions options = new ChromeOptions();
                options.PageLoadStrategy = PageLoadStrategy.Normal;
                options.AddArgument("--disable-notifications");
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-sync");
                options.AddArgument("--disbale-gpu");
                options.AddArguments("--mute-audio");
                options.AddArguments("--no-sandbox");
                //options.AddArgument("--headless");
                this.driver = new ChromeDriver(path, options);

            }
            if (browserOptions.Equals(BrowserOptions.Chrome) && alignOptions.Equals(AlignmentOptions.MobileSite))
            {
                ChromeOptions options = new ChromeOptions();
                options.PageLoadStrategy = PageLoadStrategy.Normal;
                options.EnableMobileEmulation("iPhone 6");
                options.AddArgument("--disable-notifications");
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-sync");
                options.AddArgument("--disbale-gpu");
                options.AddArguments("--mute-audio");
                options.AddArguments("--no-sandbox");
                //options.AddArgument("--headless");
                this.driver = new ChromeDriver(path, options);
            }
            if (browserOptions.Equals(BrowserOptions.Firefox) && alignOptions.Equals(AlignmentOptions.Desktop))
            {

            }
            if (browserOptions.Equals(BrowserOptions.Firefox) && alignOptions.Equals(AlignmentOptions.MobileSite))
            {

            }

            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            this.driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(100);
        }

        public static TestObject GetTestObject(BrowserOptions browserOptions, AlignmentOptions alignOptions)//factory call
        {
            try
            {
                return new TestObject(browserOptions, alignOptions);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Dispose()
        {
            this.driver.Quit();
            this.driver = null;
        }

        public void Push(string str)
        {
            if (!((str.Equals(string.Empty)) && (str == null)))
            {
                this.dataCache.Add(str.GetHashCode(), str);
            }
        }
    }

    public class Helper
    {
        public void CheckIfClickable(TestObject testObject, IWebElement element)
        {
            try
            {
                WebDriverWait explicitWait = new WebDriverWait(testObject.driver, TimeSpan.FromSeconds(3));
                explicitWait.Until(ExpectedConditions.ElementToBeClickable(element));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool CheckIfPageIsLoaded(TestObject testObject)
        {
            try
            {
                WebDriverWait explicitWait = new WebDriverWait(testObject.driver, TimeSpan.FromSeconds(100));
                return explicitWait.Until(x => (testObject.driver as IJavaScriptExecutor).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public bool CheckIfPageIsLoaded(TestObject testObject, string urlSubString)
        {
            try
            {
                FluentRoutine(testObject, urlSubString);
                if (testObject.driver.Url.Contains(urlSubString))
                {
                    if (CheckIfPageIsLoaded(testObject))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return false;
        }
        private void FluentRoutine(IWebElement testElement)
        {
            DefaultWait<IWebElement> fluentWait = new DefaultWait<IWebElement>(testElement);
            fluentWait.PollingInterval = TimeSpan.FromSeconds(2);
            fluentWait.Message = "fluent timeout";
            fluentWait.Timeout = TimeSpan.FromSeconds(4);
            fluentWait.Until(x => testElement.Displayed.Equals(true) && testElement.Enabled.Equals(true));
        }
        private void FluentRoutine(TestObject testObject, string urlSubString)
        {
            WebDriverWait fluentWait = new WebDriverWait(testObject.driver, TimeSpan.FromSeconds(10));
            fluentWait.PollingInterval = TimeSpan.FromSeconds(2);
            fluentWait.Message = "webdriver fluent timeout";

            fluentWait.Until(x => testObject.driver.Url.Contains(urlSubString));
        }
        public bool TryGetElement(TestObject testObject, By by, out IWebElement element)//check if element is visible and enabled
        {
            try
            {

                IWebElement testElement = testObject.driver.FindElement(by);
                FluentRoutine(testElement);
                if (testElement.Displayed.Equals(true) && testElement.Enabled.Equals(true))
                {
                    element = testElement;
                    testObject.elementCache.Add(by, element);
                    return true;
                }
                else
                {
                    element = null;
                    return false;//element not enabled or displayed on page
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public bool TryGetElementList(TestObject testObject, By by, out ReadOnlyCollection<IWebElement> elementList)
        {
            try
            {
                ReadOnlyCollection<IWebElement> testElementList = null;
                testElementList = testObject.driver.FindElements(by);
                if (testElementList == null)
                {
                    elementList = null;
                    return true;
                }
                foreach (IWebElement testElement in testElementList)
                {
                    FluentRoutine(testElement);
                    if (testElement.Displayed.Equals(true) && testElement.Enabled.Equals(true))
                    {
                        //testObject.elementCache.Add(by, testElement);
                    }
                    else
                    {
                        elementList = null;
                        return false;
                    }
                }
                elementList = testElementList;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public bool CheckIfElementsAreVisible(TestObject testObject, By by, out ReadOnlyCollection<IWebElement> webElements)
        {
            ReadOnlyCollection<IWebElement> testElements = testObject.driver.FindElements(by);
            foreach (IWebElement testElement in testElements)
            {
                if (!testElement.Displayed.Equals(true))
                {
                    webElements = null;
                    return false;
                }
            }
            webElements = testElements;
            return true;
        }
        public bool CheckIfElementsAreNotVisible(TestObject testObject, By by, out ReadOnlyCollection<IWebElement> webElements)
        {
            if (!CheckIfElementsAreVisible(testObject, by, out webElements))
            {
                return true;
            }
            return false;
        }
        public void TakeScreenShot(TestObject testObject)
        {
            try
            {
                Screenshot screenshot = (testObject.driver as ITakesScreenshot).GetScreenshot();
                StringBuilder imagePath = new StringBuilder();
                imagePath.Append(LocatorsRepo.ResourceManager.path);
                imagePath.Append(DateTime.Now.Year.ToString() + "_");
                imagePath.Append(DateTime.Now.Month.ToString() + "_");
                imagePath.Append(DateTime.Now.Day.ToString() + "_");
                imagePath.Append(DateTime.Now.Hour.ToString() + "_");
                imagePath.Append(DateTime.Now.Minute.ToString());
                screenshot.SaveAsFile(imagePath.ToString(), ScreenshotImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}