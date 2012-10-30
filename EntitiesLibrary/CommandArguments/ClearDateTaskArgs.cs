using System;

namespace EntitiesLibrary.CommandArguments
{
    public class ClearDateTaskArgs : IEditCommandArguments
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}