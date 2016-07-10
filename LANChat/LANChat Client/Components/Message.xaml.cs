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

            GradientStopCollection gradientStops = new GradientStopCollection();
            Color color = (Color) ColorConverter.ConvertFromString("#FF051BF7");
            var gs1 = new GradientStop(color, 0);
            color = (Color)ColorConverter.ConvertFromString("#FF49F808");
            var gs2 = new GradientStop(color, 2);
            gradientStops.Add(gs1);
            gradientStops.Add(gs2);
            messageBorder.Background = new LinearGradientBrush(gradientStops);

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
