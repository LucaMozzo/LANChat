using LANChat_Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Timers;
using Shared;
using System.Net;
using System.Collections.ObjectModel;

namespace LANChat_Client.Components
{
    public partial class UsersList : UserControl
    {
		private ObservableCollection<User> users { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
		public UsersList()
        {
            InitializeComponent();

			Client.responseReceived += Client_responseReceived;

			var timer = new System.Timers.Timer(5000);
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}

        /// <summary>
        /// Update users list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Client_responseReceived(object sender, EventArgs e)
		{
            if (e.GetType().IsEquivalentTo(typeof(Shared.Message)))
				if (((Shared.Message)e).command == Command.Users)
				{
                    Application.Current.Dispatcher.Invoke((Action)delegate
					{
                        short selected = 0;
                        users = new ObservableCollection<User>();
						userList.ItemsSource = users;

                        foreach (User u in (LinkedList<User>)((Shared.Message)e).content)
                            users.Add(u);

                        if (selected >= 0)
                            userList.SelectedIndex = selected;
                    });

					
				}

		}

        /// <summary>
        /// Ask the server for updated users list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.serverAddress), Properties.Settings.Default.port);
			Client.Start(endPoint);

			Shared.Message m = new Shared.Message();
			m.sender = IPAddress.Parse(Properties.Settings.Default.ipv6);
			m.token = Properties.Settings.Default.token;
			m.command = Command.Users;

			Client.Send(m);

			Client.Receive();
		}

    }
}
