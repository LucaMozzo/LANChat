using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LANChat_Core
{
    public class Client
    {
        //The main client socket
        public Socket clientSocket;
        //Name by which the user logs into the room
        public string strName;

        private byte[] byteData = new byte[1024];

        public void Start()
        {
            try
            {
                //We are using TCP sockets
                clientSocket = new Socket(AddressFamily.InterNetwork,
                               SocketType.Stream, ProtocolType.Tcp);

                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                //Server is listening on port 1000
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1000);

                //Connect to the server
                clientSocket.BeginConnect(ipEndPoint,
                    new AsyncCallback(OnConnect), null);
            }
            catch (Exception ex)
            {
                //error
            }
        }

        public void Send()
        {
            try
            {
                //We are using TCP sockets
                clientSocket = new Socket(AddressFamily.InterNetwork,
                               SocketType.Stream, ProtocolType.Tcp);

                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                //Server is listening on port 1000
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1000);

                //Connect to the server
                clientSocket.BeginConnect(ipEndPoint,
                    new AsyncCallback(OnConnect), null);
            }
            catch (Exception ex)
            {
               //error
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }
    }
}
