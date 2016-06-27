using System;
using System.Net;

namespace LANChat_Client
{
    /// <summary>
    /// A user connected to the server
    /// </summary>
    public class User
    {
        public String UserName { get; private set; }
        public IPAddress IP { get; private set; }

        /// <summary>
        /// Constructor to create a new user sets the given name and retrieves local IP address
        /// </summary>
        /// <param name="userName">The public username</param>
        public User(String userName)
        {
            UserName = userName;
            IP = getIPAddress();
        }

        /// <summary>
        /// Retrieves the local IPv6 address
        /// </summary>
        /// <returns></returns>
        private IPAddress getIPAddress()
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[0]; //IPv6
        }

        public override string ToString()
        {
            return UserName + ": " + IP.ToString();
        }
    }
}
