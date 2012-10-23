using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;

namespace EntitiesLibrary
{
    [TypeConverter(typeof (SetDateArgsConverter))]
    public class SetDateArgs
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
    }
}