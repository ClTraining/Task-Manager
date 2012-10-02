using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.Manager;
using TaskConsoleClient.UI;
using Xunit;

namespace TaskConsoleClient.ConreteHandlers
{
    public class ConcreteHandlerAddTask : ICommandHandler
    {
        public string TaskName { get; private set; }

        private readonly ICommandManager manager;

        public ConcreteHandlerAddTask(ICommandManager manager)
        {
            this.manager = manager;
        }

        public bool Matches(string input)
        {
            var regex=new Regex(@"^(add)\s");
            TaskName = input.Substring(4);
            return regex.IsMatch(input);
        }

        public void Execute()
        {
            var t = new List<ConcreteHandlerShowSingleTask>();
            var resultId = manager.AddTask(TaskName);
            Console.WriteLine("Task added. Task ID: " + resultId);
        }
    }

    public class ConcreteHandlerAddTaskTests
    {
        private readonly ICommandManager manager = Substitute.For<ICommandManager>();
        private readonly ConcreteHandlerAddTask handler;

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
        public void should_extract_name_from_input()
        {
            handler.Matches("add adadadadada");
            handler.TaskName.Should().Be("adadadadada");
        }

        [Fact]
        public void should_send_string_return_id()
        {
            manager.AddTask(handler.TaskName).Returns(1);
            handler.Execute();
            manager.Received().AddTask(handler.TaskName);
        }
    }
}