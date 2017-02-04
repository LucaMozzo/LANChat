using LANChat_Core;
using System.Net;
using System.Windows;
using Shared;

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

            statusLbl.Content = "Ready";
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
