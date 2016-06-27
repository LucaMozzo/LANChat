using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LANChat
{
    /// <summary>
    /// Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        private string pattern = "(\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3})";

        /// <summary>
        /// Default constructor
        /// </summary>
        public AuthenticationWindow()
        {
            InitializeComponent();
        }

        private void remoteServer_Checked(object sender, RoutedEventArgs e)
        {
            expander.IsEnabled = true;
        }

        private void remoteServer_Unchecked(object sender, RoutedEventArgs e)
        {
            expander.IsEnabled = false;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            //password may be empty but username can't
            if (usernameTxt.Text != String.Empty)
            {
                if (remoteServer.IsChecked.Value)
                    if (Regex.Match(addressTxt.Text, pattern).Success && isNumber(portTxt.Text))
                    {
                        //TODO: authenticate

                        //shows the chat window
                        MainWindow main = new MainWindow();
                        main.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("You have not entered a valid address/port combination to connect to the server", "Validation error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
            }
            else
                MessageBox.Show("The username cannot be empty", "Empty username", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private static bool isNumber(string text)
        {
            foreach (char letter in text.ToCharArray())
                if (!((int)letter >= 48 && (int)letter <= 57))
                    return false;
            return true;
        }
    }
}
