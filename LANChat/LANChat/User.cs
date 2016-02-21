using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace LANChat
{
    public class User
    {
        private String UserName { get; private set; }
        private IPAddress IP { get; private set; }

        public User(String userName)
        {
            UserName = userName;
            IP = getIPAddress();
        }

        public override string ToString()
        {
            return UserName + ": " + IP.ToString();
        }

        private IPAddress getIPAddress()
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[0]; //IPv6
        }
    }
}
