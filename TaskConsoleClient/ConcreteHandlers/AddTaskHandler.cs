using System;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.Manager;
using Xunit;

namespace TaskConsoleClient.UI.CommandHandlers
{
    public class AddTaskHandler : BaseHandler
    {
        private readonly ICommandManager manager;
        
        public AddTaskHandler(ICommandManager manager)
        {
            Pattern = @"^(add)\s";
            this.manager = manager;
        }

        public override void Execute(string input)
        {
            var name = GetParameter(input);
            var resultId = manager.AddTask(name);
            Console.WriteLine("Task added. Task ID: " + resultId);
        }

        private string GetParameter(string input)
        {
            return input.Substring(4);
        }

    }

    public class ConcreteHandlerAddTaskTests
    {
        private readonly ICommandManager manager = Substitute.For<ICommandManager>();
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