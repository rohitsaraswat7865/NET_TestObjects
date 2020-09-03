using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TestAgent;
using OpenQA.Selenium;

namespace ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StringBuilder results = new StringBuilder();
            //factory call
            TestObject testObject = TestObject.GetTestObject(BrowserOptions.Chrome,AlignmentOptions.Desktop);
            Helper helper = new Helper();
            string path;
            List<string> stringList = new List<string>();
            DataSource source = new DataSource();
            source.ExcelParser(100, stringList);
            By xpath = By.XPath("/html/body/p[1]");
            //RestSharp.RestClient client = new RestClient();

            foreach(string str in stringList)
            {
                string strI = "https://webcache.googleusercontent.com/search?q=cache:" + str;
                //IRestRequest req = new RestRequest(strI, Method.GET);
                //IRestResponse resp = client.Execute(req);
                //testObject.driver.Navigate().GoToUrl("cache:"+strI);
                string jsScript = "window.location.href = \"" + strI + "\"" ;
                (testObject.driver as IJavaScriptExecutor).ExecuteScript(jsScript);
                helper.CheckIfPageIsLoaded(testObject);
                IWebElement element;
                if(helper.TryGetElement(testObject,xpath, out element))
                {
                    if(element.Text.Contains("404") && element.Text.ToLower().Contains("error"))
                    {
                        results.AppendLine("---------------------");
                        results.AppendLine("FAILED");
                        results.AppendLine(strI);
                        
                    }
                    else
                    {
                        results.AppendLine("---------------------");
                        results.AppendLine("PASSED");
                        results.AppendLine(strI);
                        
                    }
                }
                testObject.elementCache.Clear();
            }
            DataSource.GetDomainPath(out path);
            StreamWriter file = new StreamWriter(path + "resp.txt");
            file.WriteLine(results.ToString());

            Console.ReadKey();

        }
    }
}
