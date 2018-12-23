using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace InstagramBot
{
    public class Bot
    {

        private Process _ChromeProcess;
        

        public Process ChromeProcess
        {
            get { return _ChromeProcess; }
            set { _ChromeProcess = value; }
        }
        private HtmlParser _Parser;

        public HtmlParser Parser
        {
            get { return _Parser; }
            set { _Parser = value; }
        }

        private HtmlDocument _WorkingDocument;

        public HtmlDocument WorkingDocument
        {
            get { return _WorkingDocument; }
            set { _WorkingDocument = value; }
        }

        private InputSimulator _InputSimulator;

        public InputSimulator InputSimulator
        {
            get { return _InputSimulator; }
            set { _InputSimulator = value; }
        }

        private string _BotName;
        private Client _Client;

        public Client BotClient
        {
            get { return _Client; }
            set { _Client = value; }
        }

        public string BotName
        {
            get { return _BotName; }
            set { _BotName = value; }
        }





        public Bot(string botName)
        {
            _BotName = botName;
           
            _Parser = new HtmlParser();
            _InputSimulator = new InputSimulator();
            _ChromeProcess = new Process()
            {
                StartInfo = new ProcessStartInfo("C:/Program Files (x86)/Google/Chrome/Application/chrome.exe", $"www.instagram.com/{_BotName}")
            };
            _ChromeProcess.Start();
            
            System.Threading.Thread.Sleep(3000);
            //ChromeNav.NavigateToUser("elian.ziv", _ChromeProcess);

                _Client = new Client(new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));
                _Client.Connect();
                _Client.SendFollowing("eliran.ziv", new List<string> { "abc", "def" });
           while(true)
            {
                System.Threading.Thread.Sleep(100);
            }
            
        }

        public void GetFollowers ()
        {
           
        }

        public HtmlDocument GetHtml()
        {
            ChromeNav.OpenElements(_InputSimulator);
            ChromeNav.CopyHTML(_InputSimulator);
            string clipboardText = Clipboard.GetText(TextDataFormat.Text);
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true; //not necessesory you can remove it
            browser.DocumentText = clipboardText;
            browser.Document.OpenNew(true);
            browser.Document.Write(clipboardText);
            browser.Refresh();
            return browser.Document;
        }


       

    }
}
