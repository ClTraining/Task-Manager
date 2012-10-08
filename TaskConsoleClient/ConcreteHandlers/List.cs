using System;
using System.Collections.Generic;
using ConnectToWcf;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerConsole.ConcreteHandlers
{
    public class List : BaseHandler
    {
        private readonly IClientConnection manager;

        public List(IClientConnection manager)
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
        readonly IClientConnection manager = Substitute.For<IClientConnection>();
        readonly List handler;

        public ConcreteHandlerShowAllTests()
        {
            handler = new List(manager);
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
