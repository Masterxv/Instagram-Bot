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
    public class ChromeNav
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private static bool _InterfaceOpen = false;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);


        public static void OpenInterface(InputSimulator sim)
        {
            if (!_InterfaceOpen)
            {
                sim.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, VirtualKeyCode.VK_I);
                _InterfaceOpen = true;
            }
        }
        public static void OpenElements(InputSimulator sim)
        {
            System.Threading.Thread.Sleep(1250);
            OpenInterface(sim);
            System.Threading.Thread.Sleep(1250);
            LeftClick(0, 0);
            LeftClick(1540, 130);
        }

        public static void CopyHTML(InputSimulator sim)
        {
            OpenElements(sim);
            System.Threading.Thread.Sleep(1250);
            RightClick(1600, 190);
            System.Threading.Thread.Sleep(1250);
            LeftClick(1610, 255);
            System.Threading.Thread.Sleep(1250);
            //RightClick(1610, 255);
            System.Threading.Thread.Sleep(1250);
            sim.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_A);
            System.Threading.Thread.Sleep(1250);
            //sim.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.MENU}, VirtualKeyCode.TAB);
            System.Threading.Thread.Sleep(1250);
            sim.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_C);
        }

        public static void OpenConsole(InputSimulator sim)
        {
            OpenInterface(sim);
            System.Threading.Thread.Sleep(1250);
            LeftClick(1560, 130);
        }

        public static Point GetLocationOfElementById(InputSimulator sim, string botName, string id)
        {
            OpenConsole(sim);
            sim.Keyboard.TextEntry($"element = document.getElementsById('{id}'); " +
                $"alert(elements[e].getBoundingClientRect().left + ',' + alert(elements[e].getBoundingClientRect().top );");
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            string clip = GetAlert(sim);
            return new Point((int)double.Parse(clip.Split(',')[0]), (int)double.Parse(clip.Split(',')[1]));
        }

        public static Point GetLocationOfElementByHref(InputSimulator sim, string href)
        {
            OpenInterface(sim);
            System.Threading.Thread.Sleep(1250);
            sim.Keyboard.TextEntry("function getOffset( el ) " +
                "{    " +
                "var _x = 0; " +
                "var _y = 0;" +
                "while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop))" +
                "{" +
                "_x += el.offsetLeft - el.scrollLeft;_y += el.offsetTop - el.scrollTop;" +
                "el = el.offsetParent;" +
                "}" +
                "return { left: _x+170+window.scrollX, top: _y+window.scrollY +180};}");
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            System.Threading.Thread.Sleep(1250);

            sim.Keyboard.TextEntry($"elements = document.getElementsByTagName('a'); " +
                $"for(var e in elements) {{" +
                $" if(elements[e].href == \"{href}\") {{" +
                $"alert(getOffset(elements[e]).left + ',' + getOffset(elements[e]).top)}}" +
                $"}}");
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            System.Threading.Thread.Sleep(1250);
            string clip = GetAlert(sim);



            return new Point((int)double.Parse(clip.Split(',')[0]), (int)double.Parse(clip.Split(',')[1]));
        }

        public static string GetAlert(InputSimulator sim)
        {
            LeftClick(950, 150);
            System.Threading.Thread.Sleep(500);
            sim.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_A);
            System.Threading.Thread.Sleep(25);
            sim.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_C);
            System.Threading.Thread.Sleep(25);
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            System.Threading.Thread.Sleep(25);
            return Clipboard.GetText(TextDataFormat.Text);
        }

        public static List<string> GetElementsByClassName(InputSimulator sim, string className)
        {
            List<string> result = new List<string>();
            OpenConsole(sim);
            sim.Keyboard.TextEntry($"elements = document.getElementsByClassName('notranslate'); alert(elements.length);" +
                $"for(var e in elements) {{ alert(elements[e].innerHTML)}}");
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            System.Threading.Thread.Sleep(1250);
            int count = int.Parse(GetAlert(sim));
            for (int i = 0; i < count; i++)
            {
                result.Add(GetAlert(sim));
            }
            for (int i = 0; i < 3; i++)
            {
                sim.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
                System.Threading.Thread.Sleep(1250);
            }
            return result;

        }

        public static void NavigateToUser(string userName, Process chrome)
        {
            //chrome.Kill();
            chrome = new Process()
            {
                StartInfo = new ProcessStartInfo("C:/Program Files (x86)/Google/Chrome/Application/chrome.exe", $"www.instagram.com/{userName}")
            };
            chrome.Start();
        }

        public static List<string> GetFollowers(InputSimulator sim, string botName)
        {
            List<string> result = new List<string>();
            List<string> newResult = new List<string>();
            ChromeNav.OpenConsole(sim);
            Point followersPoint = ChromeNav.GetLocationOfElementByHref(sim, $"https://www.instagram.com/{botName}/followers/");
            ChromeNav.LeftClick(followersPoint);
            //can change to point to first follower.
            Cursor.Position = new Point(650, 500);
            System.Threading.Thread.Sleep(1000);
            sim.Mouse.VerticalScroll(-5);
            int scrollAmount = 30;
            for (int i = 0; i < scrollAmount; i++)
            {
                System.Threading.Thread.Sleep(500);
                sim.Mouse.VerticalScroll(-5);
            }

            newResult = GetElementsByClassName(sim, "notranslate");
            while (newResult.Count != result.Count)
            {
                result = newResult;
                Cursor.Position = new Point(650, 500);
                scrollAmount *= 2;
                for (int i = 0; i < scrollAmount; i++)
                {
                    System.Threading.Thread.Sleep(500);
                    sim.Mouse.VerticalScroll(-5);
                }
                newResult = GetElementsByClassName(sim, "notranslate");
            }
            return result;
        }
        public static List<string> GetFollowing(InputSimulator sim, string botName)
        {
            List<string> result = new List<string>();
            List<string> newResult = new List<string>();
            ChromeNav.OpenConsole(sim);
            Point followersPoint = ChromeNav.GetLocationOfElementByHref(sim, $"https://www.instagram.com/{botName}/following/");
            ChromeNav.LeftClick(followersPoint);
            //can change to point to first follower.
            Cursor.Position = new Point(650, 500);
            System.Threading.Thread.Sleep(1000);
            sim.Mouse.VerticalScroll(-5);
            int scrollAmount = 30;
            for (int i = 0; i < scrollAmount; i++)
            {
                System.Threading.Thread.Sleep(500);
                sim.Mouse.VerticalScroll(-5);
            }

            newResult = GetElementsByClassName(sim, "notranslate");
            while (newResult.Count != result.Count)
            {
                result = newResult;
                Cursor.Position = new Point(650, 500);
                scrollAmount *= 2;
                for (int i = 0; i < scrollAmount; i++)
                {
                    System.Threading.Thread.Sleep(500);
                    sim.Mouse.VerticalScroll(-5);
                }
                newResult = GetElementsByClassName(sim, "notranslate");
            }
            return result;
        }

        public static void LeftClick(uint px, uint py)
        {
            Cursor.Position = new Point((int)px, (int)py);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)px, (uint)py, 0, 0);
        }

        public static void RightClick(uint px, uint py)
        {
            Cursor.Position = new Point((int)px, (int)py);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)px, (uint)py, 0, 0);
        }

        public static void LeftClick(Point p)
        {
            Cursor.Position = new Point((int)p.X, (int)p.Y);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.X, (uint)p.Y, 0, 0);
        }

        public static void RightClick(Point p)
        {
            Cursor.Position = new Point((int)p.X, (int)p.Y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)p.X, (uint)p.Y, 0, 0);
        }
    }
}