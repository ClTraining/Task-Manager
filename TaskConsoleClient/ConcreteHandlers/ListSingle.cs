using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ConnectToWcf;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerConsole.ConcreteHandlers
{
    public class ListSingle : BaseHandler
    {
        private readonly IClientConnection manager;

        public ListSingle(IClientConnection manager)
        {
            Pattern = @"^(list\s)(\d+)$";
            this.manager = manager;
        }

        public override void Execute(string input)
        {
            var taskId = GetParameter(input);
            var task = manager.GetTaskById(taskId);
            Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", task.Id, task.Name, task.IsCompleted ? "+" : "-");
        }

        private int GetParameter(string input)
        {
            var regex = new Regex(Pattern);
            Group g = null;
            var match = regex.Match(input);
            if (match.Success)
                g = match.Groups[2];

            return int.Parse(g.ToString());
        }

    }

    public class ConcreteHandlerShowSingleTaskTests
    {
        private readonly IClientConnection manager = Substitute.For<IClientConnection>();
        private readonly ListSingle handler;

        public ConcreteHandlerShowSingleTaskTests()
        {
            handler = new ListSingle(manager);
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
            var ct = new ContractTask { Id = 1, IsCompleted = true, Name = "bla-bla" };
            manager.GetTaskById(1).Returns(ct);

            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            handler.Execute("list 1");
            sb.ToString().Should().BeEquivalentTo("ID: 1\tTask: bla-bla\tCompleted: +\r\n");
        }
    }
}
