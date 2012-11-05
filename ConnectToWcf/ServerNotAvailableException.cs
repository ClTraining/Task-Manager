using System;

namespace ConnectToWcf
{
    public class ServerNotAvailableException : Exception
    {
        public ServerNotAvailableException() 
            : base("Server is not available.") { }
    }
}