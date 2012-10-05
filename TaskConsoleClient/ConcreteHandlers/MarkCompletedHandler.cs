using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.Manager;
using Xunit;

namespace TaskConsoleClient.UI.CommandHandlers
{
    public class MarkCompletedHandler : BaseHandler
    {
        private readonly ICommandManager manager;

        public MarkCompletedHandler(ICommandManager manager)
        {
            Pattern = @"^(complete\s)(\d+)$";
            this.manager = manager;
        }

        public override void Execute(string input)
        {
            var id = GetParameter(input);
            manager.MarkCompleted(id);
            Console.WriteLine("Task ID: {0} completed.", id);
        }

        private int GetParameter(string input)
        {
            var id = 0;
            var regex = new Regex(Pattern);
            var match = regex.Match(input);
            if (match.Success)
            {
                var group = match.Groups[2];
                id = int.Parse(group.ToString());
            }
            return id;
        }
        
    }

    public class ConcreteHandlerMarkCompletedTests
    {
        private readonly ICommandManager manager = Substitute.For<ICommandManager>();
        private readonly MarkCompletedHandler handler;

        public ConcreteHandlerMarkCompletedTests()
        {
            handler = new MarkCompletedHandler(manager);
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
