using System;

namespace EntitiesLibrary
{
    public class CouldNotSetDateException: Exception
    {
        public CouldNotSetDateException(string message): base(message)
        {
        }
    }
}
