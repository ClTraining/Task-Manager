using System;
using System.ComponentModel;

namespace EntitiesLibrary
{
    [TypeConverter(typeof (ListArgsConverter))]
    public class ListArgs
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
    }
}