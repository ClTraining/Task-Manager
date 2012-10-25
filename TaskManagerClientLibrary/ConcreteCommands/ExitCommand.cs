using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public class ExitCommand : ICommand
    {
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }
        private readonly EnvironmentWrapper manager;

        public void Execute(List<string> argument)
        {
            manager.Exit();
        }

        public ExitCommand(EnvironmentWrapper manager)
        {
            this.manager = manager;
            Description = "Exits from client.";
        }
    }

    public class ExitTests
    {
        private readonly ExitCommand handler;
        private readonly EnvironmentWrapper wrapper = Substitute.For<EnvironmentWrapper>();

        public ExitTests()
        {
            handler = new ExitCommand(wrapper);
        }

        [Fact]
        public void property_name_should_be_classname()
        {
            handler.Name.Should().Be("exit");
        }

        [Fact]
        public void should_close_application()
        {
            handler.Execute(null);

            wrapper.Received().Exit();
        }
    }
}