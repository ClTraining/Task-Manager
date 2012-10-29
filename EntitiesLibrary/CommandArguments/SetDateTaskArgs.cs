using System;

namespace EntitiesLibrary.CommandArguments
{
    public interface ISetDateTaskArgs : ICommandArguments
    {
        DateTime DueDate { get; set; }
    }

    public class SetDateTaskArgs : ISetDateTaskArgs
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
    }

}