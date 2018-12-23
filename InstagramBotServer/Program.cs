
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramBotServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server(12345);
            s.Listen();

            
while(true) {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
