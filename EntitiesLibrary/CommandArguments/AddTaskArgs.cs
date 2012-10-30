using System;

namespace EntitiesLibrary.CommandArguments
{
    public class AddTaskArgs: ICommandArguments
    {
        public string Name { get; set; }

        public DateTime DueDate { get; set; }
    }
}