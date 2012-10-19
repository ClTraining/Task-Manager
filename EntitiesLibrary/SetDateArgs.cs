using System;
using System.ComponentModel;

namespace EntitiesLibrary
{
    [TypeConverter(typeof (SetDateArgsConverter))]
    public class SetDateArgs
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
    }
}