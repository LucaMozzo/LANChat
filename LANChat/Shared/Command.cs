using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public enum Command
    {
        //credentials before connection
        Credentials,
        //when disconnected
        Logout,
        //a message to all clients
        Message,
        //a list of connected users
        Users
    }
}
