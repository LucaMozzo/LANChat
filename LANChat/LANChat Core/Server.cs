using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LANChat_Core
{
    class Server
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false); //thread signal
        private static Socket listener;
        private static byte[] byteData = new byte[1024];

        public static void Start(int port, int maxUsers)
        {
            //Init the socket (TCP)
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Bind and listen on any address
                listener.Bind(new IPEndPoint(IPAddress.Any, port));
                listener.Listen(maxUsers);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void AcceptCallBack(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            Socket clientSocket = listener.EndAccept(ar);

            //Start listening for more clients
            listener.BeginAccept(new AsyncCallback(AcceptCallBack), null);

            //Once the client connects then start receiving the commands from it
            clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(ReadCallback), clientSocket);

        }

        private static void ReadCallback(IAsyncResult ar)
        {
            Socket clientSocket = (Socket)ar.AsyncState;
            int bytesReceived = clientSocket.EndReceive(ar);

            //first 4 bytes the command
            Command command = (Command)BitConverter.ToInt32(byteData, 0);
            //next 4 bytes the length of the sender IP
            int senderLength = BitConverter.ToInt32(byteData, 4);
            //next 4 bytes the length of the message
            int messageLength = BitConverter.ToInt32(byteData, 8);

            IPAddress senderIP;
            string message;

            Console.WriteLine("Received {0} bytes.", bytesReceived);

            if (bytesReceived > 0)
            { 
                string senderStr = Encoding.UTF8.GetString(byteData, 12, senderLength);
                senderIP = IPAddress.Parse(senderStr);
                message = Encoding.UTF8.GetString(byteData, 12 + senderLength, messageLength);
            }

            switch (command)
            {
                case Command.Credentials:
                    break;

                case Command.Login:
                    break;

                case Command.Logout:
                    clientSocket.Close();
                    break;

                case Command.Message:
                    break;

                case Command.Users:
                    break;
            }
        }

        public static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        public static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the handler socket (client) from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}
