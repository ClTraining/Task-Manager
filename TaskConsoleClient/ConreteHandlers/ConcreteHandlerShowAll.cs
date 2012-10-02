using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.Manager;
using TaskConsoleClient.UI;
using Xunit;

namespace TaskConsoleClient.ConreteHandlers
{
    internal class ConcreteHandlerShowAll : ICommandHandler
    {
        private ICommandManager manager;

        public ConcreteHandlerShowAll(ICommandManager manager)
        {
            this.manager = manager;
        }

        public bool Matches(string input)
        {
            var regex = new Regex(@"^(list)$");
            return regex.IsMatch(input);
        }

        public void Execute()
        {
            manager
                .GetAllTasks()
                .ForEach(x =>
                         Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", x.Id, x.Name, x.IsCompleted ? "+" : "-"));

        }
    }
    public class ConcreteHandlerShowAllTests
    {
        readonly ICommandManager manager = Substitute.For<ICommandManager>();
        readonly ICommandHandler handler;

        public ConcreteHandlerShowAllTests()
        {
            handler = new ConcreteHandlerShowAll(manager);
        }

        [Fact]
        public void should_check_if_string_matches_pattern()
        {
            var matches = handler.Matches("list");
            matches.Should().BeTrue();
        }

        [Fact]
        public void should_execute_the_command()
        {
            manager.GetAllTasks().Returns(new List<ContractTask>());
            handler.Execute();
            manager.Received().GetAllTasks();
        }
    }
}
