using System;
using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    public class AddTaskArgs : ICommandArguments
    {
        public string Name { get; set; }

        public DateTime? DueDate { get; set; }
    }
}