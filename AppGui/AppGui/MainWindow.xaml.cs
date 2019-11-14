using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.XPath;
using mmisharp;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.Speech.Synthesis;
using multimodal;

namespace AppGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MmiCommunication mmiC;
        private GoogleSearchEngineUsingChrome sel;
        private bool orderDone = false;
        private bool orderStart = false;
        public TimeSpan MyDefaultTimeout { get; private set; }
        private Tts t;
        private ChromeDriver driver;

        public MainWindow()
        {
            t = new Tts();

            driver = new ChromeDriver();
            

            //t.Speak("2");


            openUberEatsChrome(driver);

            mmiC = new MmiCommunication("localhost", 8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            //t.Speak("3");
            mmiC.Start();


            //t.Speak("4");
            Console.WriteLine("before driver...");
            
            Console.WriteLine("after driver function...");

            //t.Speak("coccocosocoasodasdas");
            //t.Speak("5");

            //there is something wrogn with the t.Speak
            //maybe assync


            //t.Speak("olá, tudo");


        }

        private void MmiC_Message(object sender, MmiEventArgs e)
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



            Console.WriteLine(json.recognized[2].ToString());



            //decimal confidence = decimal.Parse(body.Value);


            //Console.WriteLine(body);
            //Console.WriteLine("helllooooo confidence: " + price);
            //t.Speak("boaaaaaaaaas.");

            //orderStart = true;
            //t.Speak("estou aqui");

            if ((string)json.recognized[0].ToString() == "KEY") //&& confidence > (decimal)0.75)) {
            {
                orderStart = true;
                t.Speak("Olá! O que gostaria de fazer?");
            }
            /*else if((string)json.recognized[0].ToString() == "EXIT")
            {
                orderDone = true;
                t.Speak("ok, até breve!");
                driver.Close();
                System.Environment.Exit(1);
            }*/

            if (orderStart)
            {

                //var searchBox = driver.FindElement(By.CssSelector("#search-suggestions-input"));
                switch ((string)json.recognized[1].ToString())
                {

                    case "scroll":
                        scrollSmooth();
                        break;
                    case "search":

                        break;
                    case "return":
                        var str = "https://www.ubereats.com/pt-PT/feed/?d=" + DateTime.Now.ToString("yyyy-M-dd") + "&et=870&pl=JTdCJTIyYWRkcmVzcyUyMiUzQSUyMkRFVEklMjAtJTIwRGVwYXJ0YW1lbnRvJTIwZGUlMjBFbGVjdHIlQzMlQjNuaWNhJTJDJTIwVGVsZWNvbXVuaWNhJUMzJUE3JUMzJUI1ZXMlMjBlJTIwSW5mb3JtJUMzJUExdGljYSUyMiUyQyUyMnJlZmVyZW5jZSUyMiUzQSUyMkNoSUpzVjdhcjZxaUl3MFJidHRlelhxZVI3YyUyMiUyQyUyMnJlZmVyZW5jZVR5cGUlMjIlM0ElMjJnb29nbGVfcGxhY2VzJTIyJTJDJTIybGF0aXR1ZGUlMjIlM0E0MC42MzMxNzMxMDAwMDAwMSUyQyUyMmxvbmdpdHVkZSUyMiUzQS04LjY1OTQ5MzMlN0Q%3D&ps=1&st=840";

                        driver.Navigate().GoToUrl(str);
                        break;
                }

                switch ((string)json.recognized[2].ToString()) //restaurants
                {

                    case "MCDONALDS":
                        //search mcdonalds
                        var searchBox = driver.FindElement(By.CssSelector("div[class='ay at az b0']"));
                        searchBox.Click();
                        searchBox = driver.FindElement(By.CssSelector("input[class='bn ct']"));
                        for (int i = 0; i < 20; i++)
                        {
                            searchBox.SendKeys(Keys.Backspace);
                        }
                        searchBox.SendKeys("mcdonalds ");

                        switch ((string)json.recognized[3].ToString()) //place
                        {
                            case "UNIVERSIDADE":
                                searchBox.SendKeys("universidade");
                                searchBox.SendKeys(Keys.Enter);

                                //t.Speak("Carregue no botão");

                                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[class='bz c4 bx c0 c3']")));
                                //var txtElement = driver.FindElementsByXPath("[contains(text(), 'Universidade')]");
                                var item = driver.FindElement(By.CssSelector("div[class='bz c4 bx c0 c3']"));
                                item.Click();
                                break;
                            case "FORUM":
                                searchBox.SendKeys("fórum");
                                searchBox.SendKeys(Keys.Enter);
                                break;
                            case "GLICINIAS":
                                searchBox.SendKeys("glicinias");
                                searchBox.SendKeys(Keys.Enter);
                                break;
                            case "":
                                searchBox.SendKeys(Keys.Enter);
                                //tts escolha a opção desejada
                                break;
                        }
                        
                        break;
                    case "MONTADITOS":
                        searchBox = driver.FindElement(By.CssSelector("div[class='ay at az b0']"));
                        searchBox.Click();
                        searchBox = driver.FindElement(By.CssSelector("input[class='bn ct']"));
                        for (int i = 0; i < 20; i++)
                        {
                            searchBox.SendKeys(Keys.Backspace);
                        }
                        searchBox.SendKeys("100 montaditos ");
                        
                        searchBox.SendKeys(Keys.Enter);
                        break;
                    case "PIZZAHUT":
                        searchBox = driver.FindElement(By.CssSelector("div[class='ay at az b0']"));
                        searchBox.Click();
                        searchBox = driver.FindElement(By.CssSelector("input[class='bn ct']"));
                        for (int i = 0; i < 20; i++)
                        {
                            searchBox.SendKeys(Keys.Backspace);
                        }
                        searchBox.SendKeys("pizza hut ");

                        switch ((string)json.recognized[3].ToString()) //place
                        {
                            case "UNIVERSIDADE":
                                searchBox.SendKeys("universidade");
                                break;
                            case "FORUM":
                                searchBox.SendKeys("fórum");
                                break;
                            case "GLICINIAS":
                                searchBox.SendKeys("glicinias");
                                break;
                        }
                        searchBox.SendKeys(Keys.Enter);
                        break;
                }
                
                //pergunta ao user o que quer?

            }


            switch ((string)json.recognized[4].ToString()) //options
            {

                case "":
                    break;
                case "MCDONALDS":
                    //search mcdonalds
                    //pergunta ao user o que quer?
                    break;
                case "MONTADITOS":

                    break;
                case "PIZZAHUT":

                    break;
            }

            string itemName = "";
            switch ((string)json.recognized[5].ToString()) //food on mcdonalds
            {
                case "":
                    break;
                case "Chicken Delights":
                    itemName = "Chicken Delights";
                    driver.FindElementByXPath("//*[contains(text(), '" + itemName + "')]").Click();
                    t.Speak("Deseja alterar o seu pedido?");
                    break;
                default:
                    //var txtElement = driver.FindElementsByXPath("[contains(text(), '" + (string)json.recognized[5].ToString() + "')]");
                    //txtElement.();
                    break;
                case "signatureclassic":
                    //var item = driver.FindElement(By.CssSelector("div[class='ao bn e0 af bg']"));
                    itemName = "Signature Classic";
                    driver.FindElementByXPath("//*[contains(text(), '" + itemName + "')]").Click();
                    t.Speak("Deseja alterar o seu pedido?");
                    break;
                case "signaturequeijobrie":

                    break;
                case "mcmenusignaturequeijobrie":

                    break;
                case "chickendelights":

                    break;
            }
            //driver.findElement(By.id("idOfTheElement")).click();
            
                
                
                
                
                
                

            switch ((string)json.recognized[6].ToString()) //food on mcdonalds
            {
                case "":
                    break;
                case "Bacon":
                    itemName = "SEM Bacon";
                    driver.FindElementByXPath("//*[contains(text(), '" + itemName + "')]").Click();
                    break;
                case "Alface":
                    itemName = "SEM Alface";
                    driver.FindElementByXPath("//*[contains(text(), '" + itemName + "')]").Click();
                    //var item = driver.FindElement(By.CssSelector("div[class='ao bn e0 af bg']"));
                    //item.Click();
                    break;
                case "Queijo":
                    itemName = "SEM Queijo";
                    driver.FindElementByXPath("//*[contains(text(), '" + itemName + "')]").Click();
                    break;
                case "Cebola":
                    itemName = "SEM Cebola";
                    driver.FindElementByXPath("//*[contains(text(), '" + itemName + "')]").Click();
                    break;
                case "Ketchup":
                    itemName = "SEM Ketchup";
                    driver.FindElementByXPath("//*[contains(text(), '" + itemName + "')]").Click();
                    break;
                case "Molho Grão de Mostarda":
                    itemName = "Molho Grão de Mostarda";
                    driver.FindElementByXPath("//*[contains(text(), '" + itemName + "')]").Click();
                    break;
                default:
                    
                    //var txtElement = driver.FindElementsByXPath("[contains(text(), '" + (string)json.recognized[5].ToString() + "')]");
                    //txtElement.();
                    break;
            }
        }

        public void scrollSmooth()
        {
            for (int i = 0; i < 60; i++)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0,1)", "");
            }
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

            var str = "https://www.ubereats.com/pt-PT/feed/?d=" + DateTime.Now.ToString("yyyy-M-dd") + "&et=870&pl=JTdCJTIyYWRkcmVzcyUyMiUzQSUyMkRFVEklMjAtJTIwRGVwYXJ0YW1lbnRvJTIwZGUlMjBFbGVjdHIlQzMlQjNuaWNhJTJDJTIwVGVsZWNvbXVuaWNhJUMzJUE3JUMzJUI1ZXMlMjBlJTIwSW5mb3JtJUMzJUExdGljYSUyMiUyQyUyMnJlZmVyZW5jZSUyMiUzQSUyMkNoSUpzVjdhcjZxaUl3MFJidHRlelhxZVI3YyUyMiUyQyUyMnJlZmVyZW5jZVR5cGUlMjIlM0ElMjJnb29nbGVfcGxhY2VzJTIyJTJDJTIybGF0aXR1ZGUlMjIlM0E0MC42MzMxNzMxMDAwMDAwMSUyQyUyMmxvbmdpdHVkZSUyMiUzQS04LjY1OTQ5MzMlN0Q%3D&ps=1&st=840";

            driver.Navigate().GoToUrl(str);

            // 3. Find the search textbox (by ID) on the homepage


            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button[class='ao aq b2']")));


            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //driver.Manage().Window.Minimize();
            //driver.Manage().Window.Maximize();
            //driver.FindElement(By.CssSelector("button[class='ao aq b2']")).Click(); //By.CssSelector("button[class='ao aq b2]'"));
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

        public class GoogleSearchEngineUsingChrome
        {
            public TimeSpan MyDefaultTimeout { get; private set; }

            public void Shoud_Search_Using_Chrome()
            {
                // Initialize the Chrome Driver
                using (var driver = new ChromeDriver())
                {
                    // 1. Maximize the browser
                    driver.Manage().Window.Maximize();

                    // 2. Go to the "Google" homepage
                    /*driver.Navigate().GoToUrl("https://www.ubereats.com/pt-PT/feed/?pl=JTdCJTIyYWRkcmVzcyUyMiUzQSUyMkRFVEklMjAtJTIwRGVwYXJ0YW1lbnRvJTIwZGUlMjBFbGVjdHIlQzMlQjNuaWNhJTJDJTIwVGVsZWNvbXVuaWNhJUMzJUE3JUMzJUI1ZXMlMjBlJTIwSW5mb3JtJUMzJUExdGljYSUyMiUyQyUyMnJlZmVyZW5jZSUyMiUzQSUyMkNoSUpzVjdhcjZxaUl3MFJidHRlelhxZVI3YyUyMiUyQyUyMnJlZmVyZW5jZVR5cGUlMjIlM0ElMjJnb29nbGVfcGxhY2VzJTIyJTJDJTIybGF0aXR1ZGUlMjIlM0E0MC42MzMxNzMxMDAwMDAwMSUyQyUyMmxvbmdpdHVkZSUyMiUzQS04LjY1OTQ5MzMlN0Q%3D");
                    */

                    driver.Navigate().GoToUrl("https://www.ubereats.com/pt-PT/feed/?d=2019-11-02&pl=JTdCJTIyYWRkcmVzcyUyMiUzQSUyMkRFVEklMjAtJTIwRGVwYXJ0YW1lbnRvJTIwZGUlMjBFbGVjdHIlQzMlQjNuaWNhJTJDJTIwVGVsZWNvbXVuaWNhJUMzJUE3JUMzJUI1ZXMlMjBlJTIwSW5mb3JtJUMzJUExdGljYSUyMiUyQyUyMnJlZmVyZW5jZSUyMiUzQSUyMkNoSUpzVjdhcjZxaUl3MFJidHRlelhxZVI3YyUyMiUyQyUyMnJlZmVyZW5jZVR5cGUlMjIlM0ElMjJnb29nbGVfcGxhY2VzJTIyJTJDJTIybGF0aXR1ZGUlMjIlM0E0MC42MzMxNzMxMDAwMDAwMSUyQyUyMmxvbmdpdHVkZSUyMiUzQS04LjY1OTQ5MzMlN0Q%3D&ps=1&st=1350");

                    // 3. Find the search textbox (by ID) on the homepage

                
                    //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button[class='ao aq b2']")));


                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                    driver.FindElement(By.CssSelector("button[class='ao aq b2']")).Click(); //By.CssSelector("button[class='ao aq b2]'"));
                                                                                            //driver.FindElementByCssSelector("button.ao.aq.b2");
                                                                                            //searchBox.Click();
                                                                                            //searchBox.SendKeys(Keys.Enter);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("#search-suggestions-input")));

                    var searchBox = driver.FindElement(By.CssSelector("#search-suggestions-input"));
                    searchBox.SendKeys("mcdonalds universidade");
                    searchBox.SendKeys(Keys.Enter);

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


                    while (true) ;
                }
            }
        }
    }
}
