using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InstagramBot
{
    public class Client
    {
        private IPEndPoint _EndPoint;
        private int _ServerPort;
        private Socket _Socket;

        public int ServerPort
        {
            get { return _ServerPort; }
            set { _ServerPort = value; }
        }

        public Socket Socket
        {
            get { return _Socket; }
            set { _Socket = value; }
        }

        public IPEndPoint EndPoint
        {
            get { return _EndPoint; }
            set { _EndPoint = value; }
        }

        public Client(IPEndPoint ipEndpoint) 
        {
            _EndPoint = ipEndpoint;
            _Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            _Socket.Connect(_EndPoint);
        }

        private void Send(string data)
        {
            string len = data.Length.ToString().PadLeft(5, '0');
            _Socket.Send(Encoding.UTF8.GetBytes(len + data));
        }

        public void SendFollowers(string userName, List<string> followers)
        {

            Dictionary<string, object> request = new Dictionary<string, object>()
            {
                {"request","update-followers" },
                {"user",userName },
                {"followers", followers }
            };
            Send(JsonConvert.SerializeObject(request));
        }

        public void SendFollowing(string userName, List<string> following)
        {
            Dictionary<string, object> request = new Dictionary<string, object>()
            {
                {"request","update-following" },
                {"user",userName },
                {"following", following }
            };
            Send(JsonConvert.SerializeObject(request));
        }
    }
    internal static class Extensions {
        public static string toJson(this List<string> list)
        {
            return string.Join(",", list);
        }
        public static string toJson(this Dictionary<string, string> dic)
        {
            var entries = dic.Select(d =>
                string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";

        }
    }
}
