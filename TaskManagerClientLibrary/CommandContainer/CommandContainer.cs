using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using TaskManagerClientLibrary.ConcreteCommands;
using Xunit;

namespace TaskManagerClientLibrary.CommandContainer
{
    public class CommandContainer : ICommandContainer
    {
        private readonly IEnumerable<ICommand> commands;

        public CommandContainer(IEnumerable<ICommand> commands)
        {
            this.commands = commands;
        }

        public IEnumerable<ICommand> GetCommands()
        {
            return commands;
        }

    }

    public class CommandContainerTests
    {
        [Fact]
        public void should_return_collection_of_commands()
        {
            var command = Substitute.For<ICommand>();
            var commands = new List<ICommand> {command};
            var container = new CommandContainer(commands);

            var res = container.GetCommands();

            res.Should().BeEquivalentTo(commands);
        }
    }
}