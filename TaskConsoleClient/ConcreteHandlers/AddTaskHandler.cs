using System;
using System.Text.RegularExpressions;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerConsole.ConcreteHandlers
{
    public class AddTaskHandler : ICommandHandler
    {
        private readonly IClientConnection manager;
        private const string Pattern = @"^(add)\s";

        public AddTaskHandler(IClientConnection manager)
        {
            this.manager = manager;
        }

        public bool Matches(string input)
        {
            var regex = new Regex(Pattern);
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
        private readonly IClientConnection manager = Substitute.For<IClientConnection>();
        private readonly AddTaskHandler handler;
        const string TaskName = "add 1";

        public ConcreteHandlerAddTaskTests()
        {
            handler = new AddTaskHandler(manager);
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
            handler.Execute(TaskName);
            manager.Received().AddTask("1");
        }
    }
}