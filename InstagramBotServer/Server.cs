using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InstagramBotServer
{
    public class Server
    {
        private delegate void HandleClientRequest(Socket client, JToken request);
        private Dictionary<string, HandleClientRequest> _RequestHandles;
        
        private int _Port;
        private Socket _ServerSocket;
        private Thread _ListenThread;


        public Server(int port)
        {
            _ServerSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _ServerSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            _RequestHandles = new Dictionary<string, HandleClientRequest>();
            _RequestHandles.Add("update-followers", HandleUpdateFollowers);
            _RequestHandles.Add("update-following", HandleUpdateFollowing);
        }

        public void Listen()
        {
            if (_ListenThread != null && _ListenThread.IsAlive)
            {
                _ListenThread.Abort();
            }
            _ListenThread = new Thread(() => _Listen());
            _ListenThread.Start();
        }

        private void _Listen()
        {
            _ServerSocket.Listen(5);
            while (true)
            {
                Socket s = ServerSocket.Accept();
                ThreadPool.QueueUserWorkItem(new WaitCallback(HandleClient),s);
            }
        }

        private void HandleUpdateFollowers(Socket client, JToken token)
        {
            foreach(string data in token["followers"])
            {
                Console.WriteLine(data);
            }
        }

        private void HandleUpdateFollowing(Socket client, JToken token)
        {
            foreach (JToken data in token["following"])
            {
                Console.WriteLine(data.ToString());
            }
        }

        private void HandleClient(object clientSocket)
        {
            Socket s = clientSocket as Socket;
            while (true)
            {
                try
                {
                    JToken jsonData = Recieve(s);
                    _RequestHandles[jsonData["request"].ToString()](s, jsonData);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error reading data, closing : " + s.RemoteEndPoint.ToString());
                    break;
                }
            }
            s.Disconnect(false);
        }

        private JToken Recieve(Socket s)
        {
            byte[] lenBuffer = new byte[5];
            s.Receive(lenBuffer);
            int len = int.Parse(Encoding.UTF8.GetString(lenBuffer));
            byte[] messageBuffer = new byte[len];
            s.Receive(messageBuffer);
            string message = Encoding.UTF8.GetString(messageBuffer);
            return (JToken)JsonConvert.DeserializeObject(message);
        }

        public Socket ServerSocket
        {
            get { return _ServerSocket; }
            set { _ServerSocket = value; }
        }

        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

    }
}
