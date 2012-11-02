using System;
using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    public class SetDateTaskArgs : IEditCommandArguments
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
    }
}