using System;
using System.Windows;
using System.Windows.Input;
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
        /// <summary>
        /// The constructor fucusses on controls
        /// </summary>
        public AuthenticationWindow()
        {
            InitializeComponent();

            userTxt.Focus();
        }

        /// <summary>
        /// If remote server is checked, the dropdown is enabled 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void remoteServer_Checked(object sender, RoutedEventArgs e)
        {
            addressTxt.IsEnabled = true;
        }

        /// <summary>
        /// If remote server is unchecked, the dropdown is disabled
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void remoteServer_Unchecked(object sender, RoutedEventArgs e)
        {
            addressTxt.Text = "127.0.0.1";
            addressTxt.IsEnabled = false;
        }

        /// <summary>
        /// Send a request to the specified server and authenticate
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
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
				loginBtn.IsEnabled = false;

                IPAddress addr = IPAddress.Parse(addressTxt.Text);
                IPEndPoint endPoint = new IPEndPoint(addr, Convert.ToInt16(portTxt.Text));
                Properties.Settings.Default.serverAddress = addr.ToString();
                Properties.Settings.Default.port = Convert.ToInt16(portTxt.Text);
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
            finally
            {
                loginBtn.IsEnabled = true;
            }
        }

        /// <summary>
        /// Response from the server, once received, log into the main window
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void Client_responseReceived(object sender, EventArgs e)
        {
            Properties.Settings.Default.token = (Token) ((Message)e).content;

            //to avoid exception
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MainWindow main = new MainWindow();

                Client.responseReceived -= Client_responseReceived; //unsubscribe
                Hide();
                Close();

                main.Show();
            });
        }

        /// <summary>
        /// Shortcut for login
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void userTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                loginBtn_Click(null, new RoutedEventArgs());
        }

        /// <summary>
        /// Shortcut for login
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void pwdTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                loginBtn_Click(null, new RoutedEventArgs());
        }
    }
}
