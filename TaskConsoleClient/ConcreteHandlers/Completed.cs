using System;
using System.Text.RegularExpressions;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerConsole.ConcreteHandlers
{
    public class Completed : BaseHandler
    {
        private readonly IClientConnection manager;

        public Completed(IClientConnection manager)
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
        private readonly IClientConnection manager = Substitute.For<IClientConnection>();
        private readonly Completed handler;

        public ConcreteHandlerMarkCompletedTests()
        {
            handler = new Completed(manager);
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
