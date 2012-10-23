using System;
using System.ComponentModel;

namespace EntitiesLibrary.Arguments.AddTask
{
    [TypeConverter(typeof (AddTaskArgsConverter))]
    public class AddTaskArgs
    {
        public string Name { get; set; }

        public DateTime DueDate { get; set; }
    }
}