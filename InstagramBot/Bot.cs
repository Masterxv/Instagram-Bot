using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
            ChromeNav.NavigateToUser("idf_confessions", _ChromeProcess);
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
