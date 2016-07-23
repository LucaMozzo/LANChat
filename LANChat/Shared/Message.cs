using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using LANChat_Core;

namespace Shared
{
    [Serializable]
    public class Message : EventArgs
    {
        public Command command { get; set; }
        public IPAddress sender { get; set; }
        public IPAddress receiver { get; set; }
        public Token token { get; set; }
        public object content { get; set; }
    }
}
