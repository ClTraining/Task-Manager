using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.Manager;
using Xunit;

namespace TaskConsoleClient.ConcreteHandlers
{
    public class ConcreteHandlerAddTask : ICommandHandler
    {
        private readonly ICommandManager manager;

        public ConcreteHandlerAddTask(ICommandManager manager)
        {
            this.manager = manager;
        }

        public bool Matches(string input)
        {
            var regex = new Regex(@"^(add)\s");
            return regex.IsMatch(input);
        }

        public void Execute(string input)
        {
            var name = input.Substring(4);
            var resultId = manager.AddTask(name);
            Console.WriteLine("Task added. Task ID: " + resultId);
        }
    }

    public class ConcreteHandlerAddTaskTests
    {
        private readonly ICommandManager manager = Substitute.For<ICommandManager>();
        private readonly ConcreteHandlerAddTask handler;
        const string taskName = "add 1";

        public ConcreteHandlerAddTaskTests()
        {
            handler = new ConcreteHandlerAddTask(manager);
        }

        [Fact]
        public void should_check_the_input_for_correctness()
        {
            var matches = handler.Matches("add adadadadada");

            matches.Should().BeTrue();
        }

        [Fact]
        public void should_send_string_return_id()
        {
            handler.Execute(taskName);
            manager.Received().AddTask("1");
        }
    }
}