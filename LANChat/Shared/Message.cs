using System;
using System.Net;
using LANChat_Core;

namespace Shared
{
    /// <summary>
    /// Represent a message exchanged between client and server
    /// </summary>
    [Serializable]
    public class Message : EventArgs
    {
        public Command command { get; set; }
        public IPAddress sender { get; set; }
        public string receiver { get; set; }
        public Token token { get; set; }
        public object content { get; set; }
    }
}
