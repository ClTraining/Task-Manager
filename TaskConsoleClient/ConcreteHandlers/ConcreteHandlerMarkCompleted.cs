using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.Manager;
using TaskManagerHost.WCFServer;
using Xunit;

namespace TaskConsoleClient.ConcreteHandlers
{
    public class ConcreteHandlerMarkCompleted : ICommandHandler
    {
        private readonly ICommandManager manager;
        public int ID { get; private set; }

        public ConcreteHandlerMarkCompleted(ICommandManager manager)
        {
            this.manager = manager;
        }

        public bool Matches(string input)
        {
            var regex = new Regex(@"^(complete\s)(\d+)$");

            var match = regex.Match(input);
            if (match.Success)
            {
                var group = match.Groups[2];
                ID = int.Parse(group.ToString());
            }

            return regex.IsMatch(input);
        }

        public void Execute()
        {
            manager.MarkCompleted(ID);
            Console.WriteLine("Task ID: {0} completed.", ID);
        }
    }

    public class ConcreteHandlerMarkCompletedTests
    {
        private readonly ICommandManager manager = Substitute.For<ICommandManager>();
        private readonly ConcreteHandlerMarkCompleted handler;

        public ConcreteHandlerMarkCompletedTests()
        {
            handler = new ConcreteHandlerMarkCompleted(manager);
        }

        [Fact]
        public void should_check_the_input_string_for_correctness()
        {
            var matches = handler.Matches("complete 2");

            matches.Should().BeTrue();
        }

        [Fact]
        public void should_execute_command()
        {
            handler.Execute();
            manager.Received().MarkCompleted(handler.ID);
        }
    }
}
