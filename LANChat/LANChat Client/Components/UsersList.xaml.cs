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
using System.Threading;
using Shared;
using System.Net;

namespace LANChat_Client.Components
{
    public partial class UsersList : UserControl
    {

        public UsersList()
        {
            InitializeComponent();

			Client.responseReceived += Client_responseReceived;

			Thread trd = new Thread(() => refresh());
			trd.Priority = ThreadPriority.BelowNormal;
			trd.Start();
        }

		private void Client_responseReceived(object sender, EventArgs e)
		{
			if (e.GetType().IsEquivalentTo(typeof(Shared.Message)))
				if (((Shared.Message)e).command == Command.Message)
					userList.ItemsSource = (LinkedList<User>)((Shared.Message)e).content;
		}

		private void refresh()
		{
			Timer timer = new Timer(new TimerCallback(callBack), null, 0, 500);
		}

		private void callBack(Object stateInfo)
		{
			Shared.Message m = new Shared.Message();
			m.sender = IPAddress.Parse(Properties.Settings.Default.ipv6);
			m.token = Properties.Settings.Default.token;
			m.command = Command.Users;

			Client.Send(m);

			Client.Receive();
		}

    }
}
