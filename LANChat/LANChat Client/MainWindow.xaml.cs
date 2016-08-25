using LANChat_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public MainWindow()
        {
            InitializeComponent();

            statusLbl.Content = "Ready";
        }

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
