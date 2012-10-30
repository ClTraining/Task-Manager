using System;

namespace EntitiesLibrary
{
    public class WrongTaskArgumentsException : Exception
    {
        public WrongTaskArgumentsException(string message): base(message)
        {
        }
    }
}