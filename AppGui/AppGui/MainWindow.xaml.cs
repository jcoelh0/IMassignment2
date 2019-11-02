using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using mmisharp;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;

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
            InitializeComponent();


            mmiC = new MmiCommunication("localhost",8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();

            sel = new GoogleSearchEngineUsingChrome();
            sel.Shoud_Search_Using_Chrome();


        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);

            Shape _s = null;
            switch ((string)json.recognized[0].ToString())
            {
                case "SQUARE": _s = rectangle;
                    break;
                case "CIRCLE": _s = circle;
                    break;
                case "TRIANGLE": _s = triangle;
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
            public void Shoud_Search_Using_Chrome()
            {
                // Initialize the Chrome Driver
                using (var driver = new ChromeDriver())
                {
                    // 1. Maximize the browser
                    driver.Manage().Window.Maximize();

                    // 2. Go to the "Google" homepage
                    driver.Navigate().GoToUrl("http://www.google.com");

                    // 3. Find the search textbox (by ID) on the homepage
                    var searchBox = driver.FindElementByCssSelector("input[class='gLFyf gsfi']");

                    // 4. Enter the text (to search for) in the textbox
                    searchBox.SendKeys("Automation using selenium 3.0 in C#");

                    // 5. Find the search button (by Name) on the homepage
                    var searchButton = driver.FindElementByName("btnK");

                    // 6. Click "Submit" to start the search
                    searchButton.Submit();

                    // 7. Find the "Id" of the "Div" containing results stats
                    var searchResults = driver.FindElementById("resultStats");
                }
            }
        }
    }
}
