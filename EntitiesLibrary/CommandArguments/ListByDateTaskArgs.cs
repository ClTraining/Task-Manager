using System;

namespace EntitiesLibrary.CommandArguments
{
    public class ListByDateTaskArgs:IListCommandArguments
    {
        public DateTime Date { get; set; }
    }
}
