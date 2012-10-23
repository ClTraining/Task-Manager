using System;
using System.ComponentModel;

namespace EntitiesLibrary.Arguments.SetDate
{
    [TypeConverter(typeof (SetDateArgsConverter))]
    public class SetDateArgs
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
    }
}