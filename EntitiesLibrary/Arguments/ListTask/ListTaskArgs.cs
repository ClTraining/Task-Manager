using System;
using System.ComponentModel;
using EntitiesLibrary.Arguments.ListTask;

namespace EntitiesLibrary
{
    [TypeConverter(typeof(ListTaskArgsConverter))]
    public class ListTaskArgs
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
    }
}
