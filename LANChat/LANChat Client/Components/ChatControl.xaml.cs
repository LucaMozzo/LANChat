using LANChat_Core;
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
using Shared;
using System.Threading;
using System.Net;

namespace LANChat_Client.Components
{
    /// <summary>
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {
        private LinkedList<Message> messages;
        private double maxWidth = 280;
        private Thread listeningTrd;

        public ChatControl()
        {
            InitializeComponent();

            Client.responseReceived += Client_responseReceived;

            messages = new LinkedList<Message>();
        }

        private void Client_responseReceived(object sender, EventArgs e)
        {
            if (e.GetType().IsEquivalentTo(typeof(Shared.Message)))
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    AddMessage((String)((Shared.Message)e).content, Message.MessageOwner.SERVER);
                });
        }

        private static int count = 0;

        private void AddMessage(String message, Message.MessageOwner sender)
        {
            Message m = new Message(message, sender ,maxWidth);
            m.Margin = new Thickness(5, 2.5, 5, 2.5);
            //add a row
            var rd = new RowDefinition();
            rd.Height = new GridLength(50); //default
            feed.RowDefinitions.Add(rd);

            Grid.SetRow(m, count++);
            switch(sender)
            {
                case Message.MessageOwner.SERVER:
                    Grid.SetColumn(m, 0);
                    break;
                case Message.MessageOwner.YOU:
                    Grid.SetColumn(m, 1);
                    break;
            }
            Grid.SetColumnSpan(m, 2);
            feed.Children.Add(m);

            feed.UpdateLayout();
            messages.AddLast(m);
            rd.Height = new GridLength(m.GetActualHeight());
        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (messageTxt.Text != "" && messageTxt.Text != " ")
            {
                AddMessage(messageTxt.Text, Message.MessageOwner.YOU);

                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.serverAddress), Properties.Settings.Default.port);
                Client.Start(endPoint);

                var m = new Shared.Message();
                m.command = Command.Message;
                m.sender = IPAddress.Parse(Properties.Settings.Default.ipv6);
                m.content = messageTxt.Text;
                m.token = Properties.Settings.Default.token;
                Client.Send(m);

                messageTxt.Text = "";
                Client.Receive();
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

        private void chatControl_Loaded(object sender, RoutedEventArgs e)
        {
            listeningTrd = new Thread(() => receiver());
            //listeningTrd.Start();
        }


        private void receiver()
        {
            
        }
    }
}
