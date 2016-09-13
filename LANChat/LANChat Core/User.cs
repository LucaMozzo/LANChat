using Shared;
using System;
using System.Net;

namespace LANChat_Core
{
    /// <summary>
    /// A user connected to the server
    /// </summary>
	[Serializable]
    public class User
    {
        public String UserName { get; private set; }
        public IPAddress IP { get; private set; }
        public Token token { get; private set; }
        public bool online { get; set; }

        /// <summary>
        /// Constructor to create a new user sets the given name, sets the IP address and creates a token
        /// </summary>
        /// <param name="userName">The public username</param>
        /// <param name="address">The IP address</param>
        public User(String userName, IPAddress address, bool online = false)
        {
            UserName = userName;
            IP = address;
            this.online = online;
        }

        /// <summary>
        /// Constructor to create a new user sets the given name, retrieves local IP address and creates a token
        /// </summary>
        /// <param name="userName">The public username</param>
        public User(String userName) : this(userName, GetIPAddress())
        {

        }

        /// <summary>
        /// Initializes a new user, creating a token
        /// </summary>
        public void init()
        {
            token = new Token();

            Utils.WriteColour(String.Format("User {0} with address {1} authenticated with token {2}", UserName, IP, token.signature), ConsoleColor.White);
        }

        /// <summary>
        /// Retrieves the local IPv6 address
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetIPAddress()
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[0]; //IPv6
        }

        /// <summary>
        /// String representation of an user
        /// </summary>
        /// <returns>Formatted string with name, IP and token</returns>
        public override string ToString()
        {
            return UserName + ": " + IP.ToString() + " (" + token.signature + ")";
        }
    }
}
