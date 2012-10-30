using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using TaskManagerClientLibrary.ConcreteCommands.TaskFormatter;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class Factory : IFactory
    {
        public ITaskFormatter GetFormatter(IListCommandArguments specification)
        {
            if (specification is ListSingleTaskArgs)
                return new SingleTaskFormatter();
            return new ListTaskFormatter();
        }
    }

    public class FactoryTests
    {
        private readonly Factory factory = new Factory();

        [Fact]
        public void should_return_list_formatter()
        {
            var result = factory.GetFormatter(new ListAllTaskArgs());
            result.Should().BeOfType<ListTaskFormatter>();
        }

        [Fact]
        public void should_return_single_formatter()
        {
            var result = factory.GetFormatter(new ListSingleTaskArgs());
            result.Should().BeOfType<SingleTaskFormatter>();
        }
    }
}