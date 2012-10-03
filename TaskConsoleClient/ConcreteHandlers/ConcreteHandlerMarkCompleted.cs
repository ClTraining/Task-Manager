using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.Manager;
using Xunit;

namespace TaskConsoleClient.ConcreteHandlers
{
    public class ConcreteHandlerMarkCompleted : ICommandHandler
    {
        private readonly ICommandManager manager;
        private const string Pattern = @"^(complete\s)(\d+)$";

        public ConcreteHandlerMarkCompleted(ICommandManager manager)
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
            var id = 0;
            var regex = new Regex(Pattern);
            var match = regex.Match(input);
            if (match.Success)
            {
                var group = match.Groups[2];
                id = int.Parse(group.ToString());
            }

            manager.MarkCompleted(id);
            Console.WriteLine("Task ID: {0} completed.", id);
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
            handler.Execute("complete 1");
            manager.Received().MarkCompleted(1);
        }
    }
}
