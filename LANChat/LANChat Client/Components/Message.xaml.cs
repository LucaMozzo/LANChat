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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LANChat_Client.Components
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Message : UserControl
    {

        public Message(string messageStr, double maxWidth = 280)
        { 
            InitializeComponent();

            text.MaxWidth = maxWidth;
            text.Text = messageStr;

        }

        public double GetActualHeight()
        {
            return text.ActualHeight;
        }

        public void SetMaxWidth(double newWidth)
        {
            text.MaxWidth = newWidth;
        }
    }
}
