using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using LANChat_Core;
using System.Linq;
using System.Text;
using System.Net;

namespace LANChat_Server
{
    /// <summary>
    /// Partial class of the main Program of the server. It handles all the supported requests
    /// </summary>
    partial class Program
    {
        private static LinkedList<User> onlineUsers = new LinkedList<User>();
        private static Message response;

        /// <summary>
        /// When a message is received from a client
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">The message</param>
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
                                response = new Message();
                                response.content = user.token;
                                Server.Send(response);

                                onlineUsers.AddLast(user);
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            //TODO no user found
                        }
                        break;

                    case Command.Logout:
                        if (Database.checkToken(m.token))
                        {
                            int rowsAffected = Database.ExecuteNonQuery(String.Format("DELETE FROM Session WHERE UserIP='{0}';", m.sender));
                            if (rowsAffected > 0)
							{
								Utils.WriteColour("The user with IPv6 " + m.sender + " has logged out.");

								//remove the user from the list
								onlineUsers.Remove((from u in onlineUsers where u.IP.Equals(m.sender) select u).ElementAt(0));
							}
                        }
                        break;
                    case Command.Message:
                        if (Database.checkToken(m.token))
                        {
                            response = new Message();
                            response.command = Command.Message;
                            response.content = "Received: " + (String)m.content;
                            Server.Send(response);
                        }
                        break;
                    case Command.Users:
                        if (Database.checkToken(m.token))
                        {
                            response = new Message();
                            response.command = Command.Users;
                            response.content = onlineUsers;
                            Server.Send(response);
                        }
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
