using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LANChat_Core;
using System.Net;
using Shared;
using System.Net.Sockets;

namespace LANChat_Client
{
    /// <summary>
    /// Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
        }

        private void remoteServer_Checked(object sender, RoutedEventArgs e)
        {
            addressTxt.IsEnabled = true;
        }

        private void remoteServer_Unchecked(object sender, RoutedEventArgs e)
        {
            addressTxt.Text = "127.0.0.1";
            addressTxt.IsEnabled = false;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if(userTxt.Text == "" || portTxt.Text == "" || addressTxt.Text == "")
            {
                MessageBox.Show("Check the username, the port and the server address - they can't be empty", "Data entered incorrectly", 
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;

            }

            try
            {
                IPAddress addr = IPAddress.Parse(addressTxt.Text);
                IPEndPoint endPoint = new IPEndPoint(addr, Convert.ToInt16(portTxt.Text));
                Client.Start(endPoint);
                
                //request a token
                Message requestToken = new Message();
                requestToken.command = Command.Credentials;
                requestToken.sender = User.GetIPAddress();
                Properties.Settings.Default.ipv6 = requestToken.sender.ToString();
                requestToken.content = userTxt.Text + "\n" + pwdTxt.Password;
                Client.Send(requestToken);

                //listen for token
                Client.responseReceived += Client_responseReceived;
                Client.Receive();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("The IP address of the server is not valid", "Data entered incorrectly", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show("The port must be a number", "Data entered incorrectly", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Error when trying to connect to the server. Check if the data is entered correctly", "Server unreachable", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Client_responseReceived(object sender, EventArgs e)
        {
            Properties.Settings.Default.token = (Token) ((Message)e).content;

            MainWindow main = new MainWindow();
            Hide();
            main.Show();
            Close();
        }
    }
}
