using LANChat_Core;
using System.Net;
using System.Windows;
using Shared;
using System;

namespace LANChat_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Property that changes the status when setted
        /// </summary>
        public string status
        {
            set
            {
                statusLbl.Content = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Client.notSent += Client_notSent;
            statusLbl.Content = "Ready";
        }

        /// <summary>
        /// Message that was not sent
        /// </summary>
        /// <param name="sender">Sender (null)</param>
        /// <param name="e">The message not sent. Null if it wasn't a message</param>
        private void Client_notSent(object sender, EventArgs e)
        {
            //handle this
        }

        /// <summary>
        /// Notifies the server when it closes
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.serverAddress), Properties.Settings.Default.port);
            Client.Start(endPoint);

            var m = new Message();
            m.command = Command.Logout;
            m.sender = IPAddress.Parse(Properties.Settings.Default.ipv6);
            m.token = Properties.Settings.Default.token;

            Client.Send(m);
        }

        
    }
}
