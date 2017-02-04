using Shared;
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
        private static Socket clientSocket;
        private static IPEndPoint server;
        private static byte[] buffer;

        public static event EventHandler notSent;
        public static event EventHandler responseReceived;

        public static void Start(IPEndPoint ipEndPoint)
        {
            //loads the size of the buffer from the settings
            int bufferSize = Properties.Settings.Default.bufferSize;
            buffer = new byte[bufferSize];

            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                server = ipEndPoint;

                //Connect to the server
                clientSocket.BeginConnect(server, new AsyncCallback(ConnectCallback), clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void Receive()
        {
            try
            {
                // Begin receiving the data from the remote device.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void Send(object data)
        {
            // Convert the string data to byte data
            byte[] byteData = Shared.Utils.ObjectToByteArray(data);
            
            // Begin sending the data to the remote device.
            if(clientSocket.Connected) //to avoid exception to be thrown when server disconnects
                clientSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), clientSocket);
            else
                notSent?.Invoke(null, (data is Message ? (Message)data : null));
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Read data from the remote device.
                int bytesRead = clientSocket.EndReceive(ar);

                Message m = (Message)Utils.ByteArrayToObject(buffer);

                responseReceived?.Invoke(null, m);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
