using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Shared
{
    [Serializable]
    public class Message : EventArgs
    {
        public Command command { get; set; }
        public IPAddress sender { get; set; }
        public IPAddress receiver { get; set; }
        public object content { get; set; }
    }
}
