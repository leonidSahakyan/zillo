using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZilloApi
{
    public class Commands
    {
        IWebDriver driver;
        public Commands(IWebDriver driver)
        {
            this.driver = driver;
        }
        public void Click(By by)
        {
            Wait(by);
            driver.FindElement(by).Click();

        }
        public void Wait(By by)
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            wait.Until(drw => drw.FindElement(by));
        }
        public void GetText(string text, By by)
        {
          
             Wait(by);
             text= driver.FindElement(by).Text;
            Console.WriteLine(text);
        }

    }
}
