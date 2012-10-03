using System;
using System.ServiceModel;
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
        public int ID { get; set; }
        private readonly ICommandManager manager;

        public ConcreteHandlerShowSingleTask(ICommandManager manager)
        {
            this.manager = manager;
        }

        public bool Matches(string input)
        {
            var regex = new Regex(@"^(list\s)(\d+)$");

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
            try
            {
                var task = manager.GetTaskById(ID);
                Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", task.Id, task.Name, task.IsCompleted ? "+" : "-");
            }
            catch (FaultException e)
            {
                Console.WriteLine("Task not found. Task ID: {0}", e.Message);
            }
        }
    }

    public class ConcreteHandlerShowSingleTaskTests
    {
        readonly ICommandManager manager = Substitute.For<ICommandManager>();
        readonly ConcreteHandlerShowSingleTask handler;

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
        public void should_execute_id_from_input()
        {
            handler.Matches("list 1");
            handler.ID.Should().Be(1);
        }

        [Fact]
        public void should_check_if_manager_send_request()
        {
            var task = new ContractTask { Id = 1 };
            manager.GetTaskById(1).Returns(task);

            handler.ID = manager.GetTaskById(1).Id;

            handler.Execute();
            manager.Received().GetTaskById(handler.ID);
        }
    }
}
