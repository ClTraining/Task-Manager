using System;
using System.Collections.Generic;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskConsoleClient.Manager
{
    class CommandManager : ICommandManager
    {
        private readonly IConnection conn;

        public CommandManager(IConnection conn)
        {
            this.conn = conn;
        }

        public int AddTask(string task)
        {
            var res = conn.GetClient().AddTask(task);
            return res;
        }

        public ContractTask GetTaskById(int id)
        {
            return conn.GetClient().GetTaskById(id);
        }

        public bool MarkCompleted(int task)
        {
            return conn.GetClient().Edit(task);
        }

        public List<ContractTask> GetAllTasks()
        {
            return conn.GetClient().GetAllTasks();
        }
    }

    public class CommandManagerTests
    {
        readonly ContractTask inTask = new ContractTask{Id = 1};
        readonly ContractTask outTask = new ContractTask();
        private readonly IConnection connection = Substitute.For<IConnection>();

        private readonly CommandManager commandManager;

        public CommandManagerTests()
        {
            commandManager = new CommandManager(connection);
        }

        [Fact]
        public void should_send_add_task_to_service()
        {
            connection.GetClient().AddTask("Hello").Returns(1);
            var result = commandManager.AddTask("Hello");

            // assert
            result.Should().Be(1);
        }

        [Fact]
        public void should_get_task_by_id()
        {
            connection.GetClient().GetTaskById(1).Returns(inTask);
            var result = commandManager.GetTaskById(1);

            // assert
            result.Should().Be(inTask);
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var list = new List<ContractTask> {inTask, outTask};
            connection.GetClient().GetAllTasks().Returns(list);
            var result = commandManager.GetAllTasks();

            // assert
            result.Should().BeEquivalentTo(list);
        }
    }
}