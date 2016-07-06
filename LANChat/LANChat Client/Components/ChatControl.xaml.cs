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
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {

        public event EventHandler btnSendPressed;

        private LinkedList<Message> messages;
        private double maxWidth = 280;

        public ChatControl()
        {
            InitializeComponent();

            messages = new LinkedList<Message>();
        }

        private static int count = 0;
        private void Test(String a)
        {
            Message m = new Message(a, maxWidth);
            m.Margin = new Thickness(5, 2.5, 5, 2.5);
            //add a row
            var rd = new RowDefinition();
            rd.Height = new GridLength(50);
            feed.RowDefinitions.Add(rd);

            Grid.SetRow(m, count++);
            Grid.SetColumn(m, 0);
            Grid.SetColumnSpan(m, 2);
            feed.Children.Add(m);

            feed.UpdateLayout();
            messages.AddLast(m);
            rd.Height = new GridLength(m.GetActualHeight());
        }

        public void AddMessage(string username = "", string message ="")
        {

        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            Test(messageTxt.Text);
            EventHandler handler = btnSendPressed;
            if (handler != null)
                handler.Invoke(this, new EventData(messageTxt.Text));
        }

        public class EventData : EventArgs
        {
            private string message { get; set; }

            public EventData(String message)
            {
                this.message = message;
            }
        }

        private void feed_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            maxWidth = feed.ActualWidth - 10;
            foreach (Message m in messages)
            {
                m.SetMaxWidth(maxWidth);
                m.UpdateLayout();
            }
                

        }
    }
}
