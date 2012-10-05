using System;
using System.Collections.Generic;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.Manager;
using Xunit;

namespace TaskConsoleClient.UI.CommandHandlers
{
    public class ShowAllTasksHandler : BaseHandler
    {
        private readonly ICommandManager manager;

        public ShowAllTasksHandler(ICommandManager manager)
        {
            Pattern = @"^(list)$";
            this.manager = manager;
        }

        public override void Execute(string input)
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
        readonly ShowAllTasksHandler handler;

        public ConcreteHandlerShowAllTests()
        {
            handler = new ShowAllTasksHandler(manager);
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
            handler.Execute("list 1");
            manager.Received().GetAllTasks();
        }
    }
}
