using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using TaskManagerClientLibrary.ComandContainer;
using TaskManagerClientLibrary.ConcreteHandlers;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class CommandContainer : ICommandContainer
    {
        private readonly List<ICommand> commands;

        public CommandContainer(List<ICommand> commands)
        {
            this.commands = commands;
        }

        #region ICommandContainer Members

        public List<ICommand> GetCommands()
        {
            return commands;
        }

        #endregion
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