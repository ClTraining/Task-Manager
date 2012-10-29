using System;

namespace EntitiesLibrary.CommandArguments
{
    public interface IClearDateTaskArgs : ICommandArguments
    {
        DateTime Date { get; }
    }

    public class ClearDateTaskArgs : IClearDateTaskArgs
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}