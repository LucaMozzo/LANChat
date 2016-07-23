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
        public Message(string messageStr, MessageOwner sender, double maxWidth = 280)
        { 
            InitializeComponent();

            text.MaxWidth = maxWidth;
            text.Text = messageStr;

            Paint(sender);
        }

        public double GetActualHeight()
        {
            return text.ActualHeight;
        }

        public void SetMaxWidth(double newWidth)
        {
            text.MaxWidth = newWidth;
        }

        public void Paint(MessageOwner sender)
        {
            GradientStopCollection gradientStops = new GradientStopCollection();
            GradientStop gradientStop1 = null, gradientStop2 = null;

            switch (sender)
            {
                case MessageOwner.SERVER:
                    Color color = (Color)ColorConverter.ConvertFromString("#FF051BF7");
                    gradientStop1 = new GradientStop(color, 0);
                    color = (Color)ColorConverter.ConvertFromString("#FF08D1F8");
                    gradientStop2 = new GradientStop(color, 2);
                    break;
                case MessageOwner.YOU:
                    color = (Color)ColorConverter.ConvertFromString("#FF8B8B8D");
                    gradientStop1 = new GradientStop(color, 0);
                    color = (Color)ColorConverter.ConvertFromString("#FFB2B4B4");
                    gradientStop2 = new GradientStop(color, 2);
                    break;
            }

            gradientStops.Add(gradientStop1);
            gradientStops.Add(gradientStop2);
            messageBorder.Background = new LinearGradientBrush(gradientStops, new Point(1, 0), new Point(1, 1));
        }

        /// <summary>
        /// The message owner is the sender of the message. Different owners may have different colours
        /// </summary>
        public enum MessageOwner { YOU, SERVER }

    }
}
