﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml.XPath;
using mmisharp;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.Speech.Synthesis;
using multimodal;

namespace AppBrowser
{
    class Program
    {

        private static MmiCommunication mmiC;
        private static bool orderDone = false;
        private static bool orderStart = false;
        public TimeSpan MyDefaultTimeout { get; private set; }
        private static Tts t;
        private static ChromeDriver driver;

        static void Main(string[] args)
        {
            driver = new ChromeDriver();
            t = new Tts();

            t.Speak("2");
            mmiC = new MmiCommunication("localhost", 8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            t.Speak("3");
            mmiC.Start();
            //t.Speak("4");
            Console.WriteLine("before driver...");
            openUberEatsChrome(driver);
            Console.WriteLine("after driver function...");
            t.Speak("coccocosocoasodasdas");
            t.Speak("5");

            //there is something wrogn with the t.Speak
            //maybe assync


            t.Speak("olá, tudo");

           
        }

        

        private static void MmiC_Message(object sender, MmiEventArgs e)
        {

            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            Console.WriteLine("doc:::::::::::");
            Console.WriteLine(doc);

            //foreach(XElement element in doc.Descendants("emma:emma"))

            //var x = doc.Descendants("emma:interpretation").First(rec => rec.Attribute("ID"); //.Attributes("emma:confidence").Single().Value;
            //Console.WriteLine("confidenceee::: " + x);
            //Descendant.Element("mmi:mmi").Attribute("emma:confidence").Value;


            Console.WriteLine(doc.Root);

            XNamespace xmlns = "http://www.w3.org/2008/04/mmi-arch";

            //string date = doc.Root.Element(xmlns + "emma:interpretation").Attribute("emma:confidence").Value;


            //var node = doc.XPathSelectElement("/mmi:mmi/mmi:startRequest/mmi:contentURL/mmi:data/emma:emma/emma:interpretation").Attribute("emma:confidence");

            //Console.WriteLine(node);

            //var price = doc.Element("emma:confidence");
            //XNamespace s = "http://www.w3.org/2003/04/emma";
            //XElement body = doc.Descendants(s + "confidence").FirstOrDefault();
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);



            Console.WriteLine(json.recognized[0].ToString());



            //decimal confidence = decimal.Parse(body.Value);


            //Console.WriteLine(body);
            //Console.WriteLine("helllooooo confidence: " + price);
            t.Speak("boaaaaaaaaas.");


            if((string)json.recognized[0].ToString() == "KEY") //&& confidence > (decimal)0.75)) {
            {
                orderStart = true;
                t.Speak("Olá! O que gostaria de encomendar?");
            }
            else if((string)json.recognized[0].ToString() != "EXIT")
            {
                orderDone = true;
                t.Speak("ok, até breve!");
            }

            if (orderStart)
            {
                var searchBox = driver.FindElement(By.CssSelector("#search-suggestions-input"));
                switch ((string)json.recognized[1].ToString())
                {

                    case "MCDONALDS":
                        //search mcdonalds
                        searchBox.SendKeys("mcdonalds universidade");
                        searchBox.SendKeys(Keys.Enter);
                        //pergunta ao user o que quer?
                        break;
                    case "MONTADITOS":

                        break;
                    case "PIZZAHUT":

                        break;
                }

            }
            else
            {

            }
            /*
            switch ((string)json.recognized[2].ToString())
            {

                case "MCDONALDS":
                    //search mcdonalds
                    //pergunta ao user o que quer?
                    break;
                case "MONTADITOS":
                    
                    break;
                case "PIZZAHUT":

                    break;
            }*/
            
        }


        public static void openUberEatsChrome(ChromeDriver driver)
        {
            // Initialize the Chrome Driver
            //using (driver)
            //{
                // 1. Maximize the browser
                driver.Manage().Window.Maximize();

                // 2. Go to the "Google" homepage
                /*driver.Navigate().GoToUrl("https://www.ubereats.com/pt-PT/feed/?pl=JTdCJTIyYWRkcmVzcyUyMiUzQSUyMkRFVEklMjAtJTIwRGVwYXJ0YW1lbnRvJTIwZGUlMjBFbGVjdHIlQzMlQjNuaWNhJTJDJTIwVGVsZWNvbXVuaWNhJUMzJUE3JUMzJUI1ZXMlMjBlJTIwSW5mb3JtJUMzJUExdGljYSUyMiUyQyUyMnJlZmVyZW5jZSUyMiUzQSUyMkNoSUpzVjdhcjZxaUl3MFJidHRlelhxZVI3YyUyMiUyQyUyMnJlZmVyZW5jZVR5cGUlMjIlM0ElMjJnb29nbGVfcGxhY2VzJTIyJTJDJTIybGF0aXR1ZGUlMjIlM0E0MC42MzMxNzMxMDAwMDAwMSUyQyUyMmxvbmdpdHVkZSUyMiUzQS04LjY1OTQ5MzMlN0Q%3D");
                */

                driver.Navigate().GoToUrl("https://www.ubereats.com/pt-PT/feed/?d=2019-11-02&pl=JTdCJTIyYWRkcmVzcyUyMiUzQSUyMkRFVEklMjAtJTIwRGVwYXJ0YW1lbnRvJTIwZGUlMjBFbGVjdHIlQzMlQjNuaWNhJTJDJTIwVGVsZWNvbXVuaWNhJUMzJUE3JUMzJUI1ZXMlMjBlJTIwSW5mb3JtJUMzJUExdGljYSUyMiUyQyUyMnJlZmVyZW5jZSUyMiUzQSUyMkNoSUpzVjdhcjZxaUl3MFJidHRlelhxZVI3YyUyMiUyQyUyMnJlZmVyZW5jZVR5cGUlMjIlM0ElMjJnb29nbGVfcGxhY2VzJTIyJTJDJTIybGF0aXR1ZGUlMjIlM0E0MC42MzMxNzMxMDAwMDAwMSUyQyUyMmxvbmdpdHVkZSUyMiUzQS04LjY1OTQ5MzMlN0Q%3D&ps=1&st=1350");

                // 3. Find the search textbox (by ID) on the homepage


                //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button[class='ao aq b2']")));


                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.Manage().Window.Minimize();
                driver.Manage().Window.Maximize();
                driver.FindElement(By.CssSelector("button[class='ao aq b2']")).Click(); //By.CssSelector("button[class='ao aq b2]'"));
                                                                                        //driver.FindElementByCssSelector("button.ao.aq.b2");
                                                                                        //searchBox.Click();
                                                                                        //searchBox.SendKeys(Keys.Enter);
                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
               
                //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("#search-suggestions-input")));

                

                
                

                /*

                new WebDriverWait(driver, MyDefaultTimeout).Until(
                d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));


                searchBox.SendKeys("DETI");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("location-enter-address-item-0")));

                searchBox.SendKeys(Keys.Enter);
                */

                //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("location-enter-address-item-0")));

                //IWebElement deti = driver.FindElementByCssSelector("button[type='submit']");
                //deti.Click();
                // 6. Click "Submit" to start the search
                //searchBox.SendKeys(Keys.Enter);

                // 7. Find the "Id" of the "Div" containing results stats
                //var searchResults = driver.FindElementById("resultStats");

                //Console.ReadKey();
           // }
        }
    }
}
