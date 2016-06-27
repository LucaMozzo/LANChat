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
    public partial class UsersList : UserControl
    {
        public List<User> users { get; }

        public UsersList()
        {
            InitializeComponent();
            users = new List<User>();
            //test only
            users.Add(new User("Test1"));
            users.Add(new User("Test2"));
            userList.ItemsSource = users;
        }

        /// <summary>
        /// Add a user to the list of connected users
        /// </summary>
        /// <param name="user">User to be included in the list</param>
        public void UserConnected(User user)
        {
            users.Add(user);
        }

        /// <summary>
        /// Remove a user from disconnected users
        /// </summary>
        /// <param name="user">User to be removed</param>
        public void UserDisconnected(User user)
        {
            users.Remove(user);
        }
    }
}
