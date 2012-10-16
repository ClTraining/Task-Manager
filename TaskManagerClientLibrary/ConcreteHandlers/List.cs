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
    public class List : Command<int?>
    {
        private readonly TaskFormatterFactory taskFormatterFactory;

        public List(IClientConnection client, ArgumentConverter<int?> converter, TextWriter textWriter, TaskFormatterFactory taskFormatterFactory)
            : base(client, converter, textWriter)
        {
            this.taskFormatterFactory = taskFormatterFactory;
        }

        protected override void ExecuteWithGenericInput(int? input)
        {
            if (input == null)
                GetTasksAndPrint(s => s.GetAllTasks(), taskFormatterFactory.GetListFormatter());
            else
                GetTasksAndPrint(s => s.GetTaskById(input.Value), taskFormatterFactory.GetSingleFormatter());
        }

        private void GetTasksAndPrint(Func<IClientConnection, List<ContractTask>> func, ITaskFormatter formatter)
        {
            var tasks = func(client);
            OutText(formatter.Show(tasks));
        }

    }

    public class ListTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ArgumentConverter<int?> converter = Substitute.For<ArgumentConverter<int?>>();
        private readonly SingleTaskFormatter formatter1 = Substitute.For<SingleTaskFormatter>();
        private readonly ListTaskFormatter formatter2 = Substitute.For<ListTaskFormatter>();
        private readonly TaskFormatterFactory taskFormatterFactory;
        private readonly List list;

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
            converter.Convert("1").Returns(1);
            var taskList = new List<ContractTask> { new ContractTask { Id = 1, Name = "some", IsCompleted = false } };
            client.GetTaskById(1).Returns(taskList);
            list.Execute("1");
            formatter1.Received().Show(taskList);
        }

        [Fact]
        public void should_execute_in_client_receiving_show_all_tasks()
        {
            converter.Convert("").Returns((int?)null);
            list.Execute("");
            client.Received().GetAllTasks();
        }

        [Fact]
        public void should_check_receiving_all_task()
        {
            converter.Convert("").Returns((int?)null);
            var taskList = new List<ContractTask>
                               {
                                   new ContractTask { Id = 1, Name = "task1", IsCompleted = false },
                                   new ContractTask{Id = 2, Name = "task2", IsCompleted = true}
                               };
            client.GetAllTasks().Returns(taskList);
            list.Execute("");
            formatter2.Received().Show(taskList);
        }

        [Fact]
        public void should_execute_in_client_receiving_show_one_tasks()
        {
            converter.Convert("1").Returns(1);
            list.Execute("1");
            client.Received().GetTaskById(1);
        }
    }

}