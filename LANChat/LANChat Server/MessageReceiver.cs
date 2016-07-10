using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using LANChat_Core;
using System.Text;

namespace LANChat_Server
{
    partial class Program
    {
        private static List<User> users = new List<User>();

        private static void Server_MessageReceived(object sender, EventArgs e)
        {
            Message m = (Message)e;

            try
            {
                switch (m.command)
                {
                    case Command.Credentials:
                        //check credentials and start a session
                        string username = ((string)m.content).Split('\n')[0];
                        string password = ((string)m.content).Split('\n')[1];
                        User user;
                        try
                        {
                            DataRow row = Database.ExecuteQuery("SELECT Password, ID FROM Users WHERE Username='" + username + "';").Rows[0];
                            if (((string)row.ItemArray[0]).Equals(password))
                            {
                                user = new User(username, m.sender);
                                string query = String.Format("INSERT INTO Session VALUES({0},'{1}','{2}');", row.ItemArray[1], user.token.signature, user.IP);
                                Database.ExecuteNonQuery(query);

                                //send the token to the client
                                Message response = new Message();
                                response.content = user.token;
                                Server.Send(response);

                                users.Add(user);
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            //TODO no user found
                        }
                        break;

                    case Command.Login:
                        break;
                    case Command.Logout:
                        break;
                    case Command.Message:
                        break;
                    case Command.Users:
                        //TODO check token first
                        m = new Message();
                        m.command = Command.Users;
                        m.content = users;
                        Server.Send(m);
                        break;
                }
            }
            catch(NullReferenceException ex)
            {
                //message > buffer
            }
        }

    }
}
