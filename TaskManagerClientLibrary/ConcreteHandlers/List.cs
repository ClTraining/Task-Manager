using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary;
using NSubstitute;
using TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class List : Command<ListArgs>
    {
        private readonly TaskFormatterFactory taskFormatterFactory;

        public List(IClientConnection client, ArgumentConverter<ListArgs> converter, TextWriter textWriter,
                    TaskFormatterFactory taskFormatterFactory)
            : base(client, converter, textWriter)
        {
            Description = "Displays list of all tasks or single task, specified by ID.";
            this.taskFormatterFactory = taskFormatterFactory;
        }

        protected override void ExecuteWithGenericInput(ListArgs input)
        {
            if (input.Id == null)
                GetTasksAndPrint(s => s.GetAllTasks(), taskFormatterFactory.GetListFormatter());
            else
                GetTasksAndPrint(s => s.GetTaskById(input.Id.Value), taskFormatterFactory.GetSingleFormatter());
        }

        private void GetTasksAndPrint(Func<IClientConnection, List<ContractTask>> func, ITaskFormatter formatter)
        {
            List<ContractTask> tasks = func(client);
            OutText(formatter.Show(tasks));
        }
    }

    public class ListTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ArgumentConverter<ListArgs> converter = Substitute.For<ArgumentConverter<ListArgs>>();
        private readonly SingleTaskFormatter formatter1 = Substitute.For<SingleTaskFormatter>();
        private readonly ListTaskFormatter formatter2 = Substitute.For<ListTaskFormatter>();
        private readonly List list;
        private readonly TaskFormatterFactory taskFormatterFactory;

        public ListTests()
        {
            taskFormatterFactory = Substitute.For<TaskFormatterFactory>(formatter1, formatter2);
            list = new List(client, converter, new StringWriter(), taskFormatterFactory);
            taskFormatterFactory.GetSingleFormatter().Returns(formatter1);
            taskFormatterFactory.GetListFormatter().Returns(formatter2);
        }

        [Fact]
        public void should_check_receiving_one_task()
        {
            var argument = new List<string> {"1"};
            converter.Convert(argument).Returns(new ListArgs {Id = 1});
            var taskList = new List<ContractTask> {new ContractTask {Id = 1, Name = "some", IsCompleted = false}};
            client.GetTaskById(1).Returns(taskList);
            list.Execute(argument);
            formatter1.Received().Show(taskList);
        }

        [Fact]
        public void should_execute_in_client_receiving_show_all_tasks()
        {
            var argument = new List<string> {""};
            converter.Convert(argument).Returns(new ListArgs {Id = null});
            list.Execute(argument);
            client.Received().GetAllTasks();
        }

        [Fact]
        public void should_check_receiving_all_task()
        {
            var argument = new List<string> {""};
            converter.Convert(argument).Returns(new ListArgs {Id = null});
            var taskList = new List<ContractTask>
                               {
                                   new ContractTask {Id = 1, Name = "task1", IsCompleted = false},
                                   new ContractTask {Id = 2, Name = "task2", IsCompleted = true}
                               };
            client.GetAllTasks().Returns(taskList);
            list.Execute(argument);
            formatter2.Received().Show(taskList);
        }

        [Fact]
        public void should_execute_in_client_receiving_show_one_tasks()
        {
            var arguments = new List<string> {"1"};
            converter.Convert(arguments).Returns(new ListArgs {Id = 1});
            list.Execute(arguments);
            client.Received().GetTaskById(1);
        }
    }
}