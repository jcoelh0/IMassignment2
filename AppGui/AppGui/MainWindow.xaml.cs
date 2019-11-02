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

        public MainWindow()
        {
            //InitializeComponent();
            Tts t = new Tts();

            t.Speak("olá, tudo");

            mmiC = new MmiCommunication("localhost",8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;

            mmiC.Start();

            //sel = new GoogleSearchEngineUsingChrome();
            //sel.Shoud_Search_Using_Chrome();


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


            var node = doc.XPathSelectElement("/mmi:mmi/mmi:startRequest/mmi:contentURL/mmi:data/emma:emma/emma:interpretation").Attribute("emma:confidence");

            Console.WriteLine(node);

            //var price = doc.Element("emma:confidence");
            //XNamespace s = "http://www.w3.org/2003/04/emma";
            //XElement body = doc.Descendants(s + "confidence").FirstOrDefault();
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);



            Console.WriteLine(json.recognized[0].ToString());



            //decimal confidence = decimal.Parse(body.Value);

            Shape _s = null;

            //Console.WriteLine(body);
            //Console.WriteLine("helllooooo confidence: " + price);



            while (!((string)json.recognized[0].ToString() == "KEY" )){ //&& confidence > (decimal)0.75)) {
                
            }

            //Tts t = new Tts();

            switch ((string)json.recognized[2].ToString())
            {

                case "MCDONALDS":

                    break;
                case "MONTADITOS":
                    _s = circle;
                    break;
                case "PIZZAHUT":
                    _s = triangle;
                    break;
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                switch ((string)json.recognized[1].ToString())
                {
                    case "GREEN":
                        _s.Fill = Brushes.Green;
                        break;
                    case "BLUE":
                        _s.Fill = Brushes.Blue;
                        break;
                    case "RED":
                        _s.Fill = Brushes.Red;
                        break;
                }
            });
            


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
