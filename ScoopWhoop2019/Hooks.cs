using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using LocatorsRepo;
using TestAgent;
using NUnit.Framework;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace SccopWhoopTestScript
{
    [Binding]
    public sealed class Hook
    {
        ExtentReports extentReport;
        ExtentTest testScenario;


        public Hook()
        {
            #region Report Directory
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;
            string reportPath = projectPath + "Reports/ExtentReport";

            #endregion

            ExtentHtmlReporter htmlreporter = new ExtentHtmlReporter(reportPath);
            this.extentReport = new ExtentReports();

            //ExtentReports extent = new ExtentReports();
            this.extentReport.AnalysisStrategy = AnalysisStrategy.BDD;
            this.extentReport.AddSystemInfo("Host Name", "ScoopWhoopQA");
            this.extentReport.AddSystemInfo("Environment", "ScoopWhoopVM");
            this.extentReport.AddSystemInfo("Username", "rohit.saraswat@scoopwhoop.com");
            this.extentReport.AttachReporter(htmlreporter);
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            try
            {
                //thats a single page application so driver can exist at feature level
                FeatureContext.Current.Add("locators", ResourceManager.GetLocators(SupportedResourceType.xpath));
                FeatureContext.Current.Add("configElements", ResourceManager.GetConfiguration());
                FeatureContext.Current.Add("testObject", TestObject.GetTestObject(BrowserOptions.Chrome, AlignmentOptions.MobileSite));
                string url = (FeatureContext.Current["configElements"] as IDictionary<string, string>)["GoogleSearch"];
                FeatureContext.Current.Add("url", url);
                TestObject testObject = FeatureContext.Current["testObject"] as TestObject;
                testObject.driver.Navigate().GoToUrl(url);

            }
            catch (Exception ex)
            {
                (FeatureContext.Current["testObject"] as TestObject).Dispose();
                Assert.Fail(ex.Message);
            }

        }
        [BeforeScenario]
        public void BeforeScenario()
        {
            string scenarioTitle = ScenarioContext.Current.ScenarioInfo.Title;
            string scenarioDescription = ScenarioContext.Current.ScenarioInfo.Description;
            this.testScenario = this.extentReport.CreateTest(scenarioTitle + "_" + scenarioDescription);
        }

        [BeforeStep]
        public void BeforeStep()
        {

        }

        [AfterStep]
        public void AfterStep()
        {

        }

        [AfterScenario]
        public void AfterScenario()
        {


            if (ScenarioContext.Current.ScenarioExecutionStatus.Equals(ScenarioExecutionStatus.OK))
            {
                this.testScenario.Log(Status.Pass, TestContext.CurrentContext.Result.Message);

            }
            else if ((ScenarioContext.Current.ScenarioExecutionStatus.Equals(ScenarioExecutionStatus.TestError)))
            {
                this.testScenario.Log(Status.Fail, TestContext.CurrentContext.Result.Message);
                this.testScenario.Log(Status.Info, TestContext.CurrentContext.Result.StackTrace);

                this.testScenario.AddScreenCaptureFromPath("");

            }
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            (FeatureContext.Current["testObject"] as TestObject).Dispose();

        }



        ~Hook()
        {
            this.extentReport.Flush();
        }

        public static void TakeScreenShot(TestObject testObject)
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
