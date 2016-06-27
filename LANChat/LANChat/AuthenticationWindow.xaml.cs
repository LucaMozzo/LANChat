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

namespace LANChat
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
            expander.IsEnabled = true;
        }

        private void remoteServer_Unchecked(object sender, RoutedEventArgs e)
        {
            expander.IsEnabled = false;
        }
    }
}
