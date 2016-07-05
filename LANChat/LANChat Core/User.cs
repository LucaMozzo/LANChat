﻿using System;
using System.Net;

namespace LANChat_Core
{
    /// <summary>
    /// A user connected to the server
    /// </summary>
    public class User
    {
        public String UserName { get; private set; }
        public IPAddress IP { get; private set; }
        public Token token { get; private set; }

        /// <summary>
        /// Constructor to create a new user sets the given name, retrieves local IP address and creates a token
        /// </summary>
        /// <param name="userName">The public username</param>
        public User(String userName, IPAddress address)
        {
            UserName = userName;
            IP = address;
            token = new Token();

            Console.WriteLine("User {0} with address {1} authenticated with token {2}", userName, IP, token.signature);
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