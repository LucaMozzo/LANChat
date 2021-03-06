﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Shared;

namespace LANChat_Core
{
    public class Server
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false); //thread signal
        private static Socket listener;
        private static byte[] buffer;
        private static Socket clientSocket;

		public static long totalSent { get; private set; }
		public static long totalReceived { get; private set; }

        public static void Start(int port, int maxUsers)
        {
            //Init the socket (TCP)
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //loads the size of the buffer from the settings
            int bufferSize = Properties.Settings.Default.bufferSize;
            buffer = new byte[bufferSize];

            try
            {
                //Bind and listen on any address
                listener.Bind(new IPEndPoint(IPAddress.Any, port));
                listener.Listen(maxUsers);

                while(true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (ThreadAbortException e) { }
            catch (SocketException e) {
                Utils.WriteColour("Exception: " + e.Message + ".\nRestart the application or try starting the server on a different port.", ConsoleColor.Red);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void AcceptCallBack(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            clientSocket = listener.EndAccept(ar);

            //Start listening for more clients
            listener.BeginAccept(new AsyncCallback(AcceptCallBack), null);

            //Once the client connects then start receiving the commands from it
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), clientSocket);

        }

        public static event EventHandler MessageReceived;

        private static void ReadCallback(IAsyncResult ar)
        {
            clientSocket = (Socket)ar.AsyncState;
            int bytesReceived = clientSocket.EndReceive(ar);

			totalReceived += bytesReceived;
            //Utils.WriteColour(String.Format("<<< Received {0} bytes.", bytesReceived), ConsoleColor.DarkGreen);

            Message message = (Message) Utils.ByteArrayToObject(buffer);

            MessageReceived?.Invoke(null, message); //raise an event
        }

        public static void Send(object data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Utils.ObjectToByteArray(data);

            // Begin sending the data to the remote device.
            clientSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), clientSocket);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the handler socket (client) from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
				totalSent += bytesSent;
                //Utils.WriteColour(String.Format(">>> {0} bytes sent.", bytesSent), ConsoleColor.DarkRed);

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
