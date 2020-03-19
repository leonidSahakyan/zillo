using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
using System.Threading;
using System.Net;
using System.Drawing;
using OpenQA.Selenium.Support.Events;

namespace ZilloApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageApiController : ControllerBase
    {
        [HttpGet]
        public string Get(string path = "C:\\Users\\Owner\\Desktop\\google_addresses1.csv")   //csv-i path  petqa poxes qo kompi path@ dnes
        {
            ZillowRead(path);
            return "Its ok";
        }
        public void ZillowRead(string filename)
        {
            string CaptchaSrc = "";
            string folderName = @"C:\Users\Owner\Desktop\Example"; //inj vor miat example papka stexci
            string csvfilename =Path.GetFileNameWithoutExtension(filename);           
            string pathString = System.IO.Path.Combine(folderName, csvfilename);
            System.IO.Directory.CreateDirectory(pathString);

            string ActualFolder = folderName + "\\" + csvfilename + "\\";
            var driverService = FirefoxDriverService.CreateDefaultService(@"C:\Users\Owner\Desktop\geckodriver\", "geckodriver.exe");
            driverService.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            driverService.HideCommandPromptWindow = true;
            driverService.SuppressInitialDiagnosticInformation = true;
            IWebDriver driver = new FirefoxDriver(driverService, new FirefoxOptions());
            using (var reader = new StreamReader(filename))
            {
                List<string> listA = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    listA.Add(line);
                }             
                for (int i = 1; i < listA.Count; i++)
                {
                    string f = listA[i].ToString();
                    string Urls = "https://www.zillow.com/homes/";
                    string[] words = f.Split(',');
                    string add = words[2];
                    System.Console.WriteLine(add);
                    words[2] = words[2].Replace(' ', '-');
                    System.Console.WriteLine(words[2]);
                    Urls += words[2] + "," + words[4] + ",-" + words[5] + "_rb/?mmlb=g,0";
                    string ZillowTitleInExcel = add + "," + " " + words[3] + "," + " " + words[4] + " " + words[5] + " | Zillow";               
                    driver.Url = Urls;
                    string UrlValue = driver.Url;
                    UrlValue = UrlValue.Substring(UrlValue.Length - 3, 3); 
                    string CompareUrl = Urls.Substring(Urls.Length - 3, 3); 
                    string Title = driver.Title;
                    if (UrlValue == CompareUrl && Title == ZillowTitleInExcel)
                    {
                        try
                        {
                            string address = driver.FindElement(By.CssSelector(".hdp-home-header-st-addr")).Text;
                            CaptchaSrc = driver.FindElement(By.CssSelector(".hdp-gallery-image-content>.hdp-gallery-image>div>img")).GetAttribute("src");  //vercnuma img-i attribut@

                            //var client = new System.Net.WebClient();
                            //string downloadPath = ActualFolder + address + ".jpg";
                            //client.DownloadFile(new Uri(CaptchaSrc), downloadPath);

                            WebClient client = new WebClient();
                            Stream stream = client.OpenRead(CaptchaSrc);
                            var bitmap = new Bitmap(stream); 
                            stream.Flush();
                            stream.Close();
                            client.Dispose();
                            if (bitmap != null)
                            {
                                bitmap.Save(ActualFolder + address + ".jpg");
                            }
                        }
                        catch (NoSuchElementException)
                        {
                            string address = driver.FindElement(By.CssSelector(".ds-address-container>span")).Text;
                            CaptchaSrc = driver.FindElement(By.CssSelector(".hdp-photo-gallery-lightbox-content>div>div>.hdp-gallery-image-content>.hdp-gallery-image>div>.sc-gzVnrw>img")).GetAttribute("src");
                            System.Console.WriteLine("errror");
                            WebClient client = new WebClient();
                            Stream stream = client.OpenRead(CaptchaSrc);

                            var  bitmap = new Bitmap(stream);
                            stream.Flush();
                            stream.Close();
                            client.Dispose();

                            if (bitmap != null)
                            {
                                bitmap.Save(ActualFolder + address + ".jpg");
                            }
                        }
                    }
                    else
                    {                        
                        driver.Close();
                        driverService = FirefoxDriverService.CreateDefaultService(@"C:\Users\Owner\Desktop\geckodriver\", "geckodriver.exe");
                        driverService.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                        driverService.HideCommandPromptWindow = true;
                        driverService.SuppressInitialDiagnosticInformation = true;
                        driver = new FirefoxDriver(driverService, new FirefoxOptions());
                    }
                }
            }
        }
    }
}