using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.Manager;
using Xunit;

namespace TaskConsoleClient.ConcreteHandlers
{
    public class ConcreteHandlerShowSingleTask : ICommandHandler
    {
        private readonly ICommandManager manager;

        public ConcreteHandlerShowSingleTask(ICommandManager manager)
        {
            this.manager = manager;
        }

        public bool Matches(string input)
        {
            var regex = new Regex(@"^(list\s)(\d+)$");

            return regex.IsMatch(input);
        }

        public void Execute(string input)
        {
            var taskId = 0;
            var regex = new Regex(@"^(list\s)(\d+)$");

            var match = regex.Match(input);
            if (match.Success)
            {
                var group = match.Groups[2];
                taskId = int.Parse(group.ToString());
            }
            var task = manager.GetTaskById(taskId);
            Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", task.Id, task.Name, task.IsCompleted ? "+" : "-");
        }
    }

    public class ConcreteHandlerShowSingleTaskTests
    {
        private readonly ICommandManager manager = Substitute.For<ICommandManager>();
        private readonly ConcreteHandlerShowSingleTask handler;

        public ConcreteHandlerShowSingleTaskTests()
        {
            handler = new ConcreteHandlerShowSingleTask(manager);
        }

        [Fact]
        public void should_check_if_command_is_correct()
        {
            var res = handler.Matches("list 1");
            res.Should().BeTrue();
        }

        [Fact]
        public void should_check_if_command_is_incorrect()
        {
            var res = handler.Matches("lista 1");
            res.Should().BeFalse();
        }

        [Fact]
        public void should_check_if_manager_send_request()
        {
            var ct = new ContractTask {Id = 1, IsCompleted = true, Name = "bla-bla"};
            manager.GetTaskById(1).Returns(ct);

            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            handler.Execute("list 1");
            sb.ToString().Should().BeEquivalentTo("ID: 1\tTask: bla-bla\tCompleted: +\r\n");
        }
    }
}
