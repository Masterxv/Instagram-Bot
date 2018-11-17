using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace InstagramBot
{
    public class HtmlParser
    {
        public HtmlParser()
        {
            
        }

        public HtmlDocument GetHtmlDocument(string url)
        {
            using (WebBrowser browser = new WebBrowser())
            {
                browser.ScrollBarsEnabled = false;
                browser.AllowNavigation = true;
                browser.Navigate(url);
                browser.Width = 1024;
                browser.Height = 768;
                browser.DocumentCompleted += Browser_DocumentCompleted;
                while (browser.ReadyState != WebBrowserReadyState.Complete)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                return browser.Document;
            }
            return null;
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        public int GetXOffset(System.Windows.Forms.HtmlElement el)
        {
            //get element pos
            int xPos = el.OffsetRectangle.Left;

            //get the parents pos
            System.Windows.Forms.HtmlElement tempEl = el.OffsetParent;
            while (tempEl != null)
            {
                xPos += tempEl.OffsetRectangle.Left;
                tempEl = tempEl.OffsetParent;
            }

            return xPos;
        }

        public Rectangle GetAbsoluteRectangle(HtmlElement element)
        {
            //get initial rectangle
            Rectangle rect = element.OffsetRectangle;

            //update with all parents' positions
            HtmlElement currParent = element.OffsetParent;
            while (currParent != null)
            {
                rect.Offset(currParent.OffsetRectangle.Left, currParent.OffsetRectangle.Top);
                currParent = currParent.OffsetParent;
            }

            return rect;
        }

        public int GetYOffset(System.Windows.Forms.HtmlElement el)
        {
            //get element pos
            int yPos = el.OffsetRectangle.Top;

            //get the parents pos
            System.Windows.Forms.HtmlElement tempEl = el.OffsetParent;
            while (tempEl != null)
            {
                yPos += tempEl.OffsetRectangle.Top;
                tempEl = tempEl.OffsetParent;
            }

            return yPos;
        }
    }
}
