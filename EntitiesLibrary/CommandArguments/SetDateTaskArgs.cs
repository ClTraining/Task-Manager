using System;

namespace EntitiesLibrary.CommandArguments
{
    public class SetDateTaskArgs : IEditCommandArguments
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
    }

}