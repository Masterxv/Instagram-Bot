using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramBotServer
{
    public static class DbLogger
    {
        private static OleDbConnection _Conn;
        
        public static void Initialize()
        {
            _Conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Elira\source\repos\Instagram-Bot\InstagramBotServer\InstagramData.accdb;Persist Security Info=True");

        }
    }
}
