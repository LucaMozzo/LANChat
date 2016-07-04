using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Shared
{
    class Message<T>
    {
        public Command command { get; private set; }
        public IPAddress sender { get; private set; }
        public IPAddress receiver { get; private set; }
        public T content { get; private set; }
    }
}
