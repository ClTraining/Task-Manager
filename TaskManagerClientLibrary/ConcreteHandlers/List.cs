using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConnectToWcf;
using EntitiesLibrary;
using NSubstitute;
using TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class List : Command<int?>
    {
        private readonly List<ITaskFormatter> formatters;

        public List(IClientConnection client, ArgumentConverter<int?> converter, TextWriter textWriter, List<ITaskFormatter> formatters)
            : base(client,  converter, textWriter)
        {
            this.formatters = formatters;
        }

        protected override void ExecuteWithGenericInput(int? input)
        {
            var taskFormatter = formatters.FirstOrDefault(f => f.CouldUse(input));

            if (input == null)
                ExecutePr(s => s.GetAllTasks(), taskFormatter);
            else
                ExecutePr(s => s.GetTaskById(input.Value), taskFormatter);
        }

        private void ExecutePr(Func<IClientConnection, List<ContractTask>> func,ITaskFormatter formatter)
        {
            var tasks = func(client);
            OutText(formatter.Show(tasks));
        }
    }

    public class ListTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ArgumentConverter<int?> converter = Substitute.For<ArgumentConverter<int?>>(); 
        private readonly ITaskFormatter formatter1 = Substitute.For<ITaskFormatter>();
        private readonly ITaskFormatter formatter2 = Substitute.For<ITaskFormatter>();
        private readonly List list;

        public ListTests()
        {

            list = new List(client, converter, new StringWriter(), new List<ITaskFormatter> { formatter1, formatter2 });
        }

        [Fact]
        public void should_check_receiving_one_task()
        {
            converter.Convert("1").Returns(1);
            var taskList = new List<ContractTask> { new ContractTask { Id = 1, Name = "some", IsCompleted = false } };
            formatter1.CouldUse(1).Returns(true);
            client.GetTaskById(1).Returns(taskList);
            list.Execute("1");
            formatter1.Received().Show(taskList);
        }

        [Fact]
        public void should_execute_in_client_receiving_show_all_tasks()
        {
            converter.Convert("").Returns((int?) null);
            formatter2.CouldUse(null).Returns(true);
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
            formatter2.CouldUse(null).Returns(true);
            client.GetAllTasks().Returns(taskList);
            list.Execute("");
            formatter2.Received().Show(taskList);
        }

        [Fact]
        public void should_execute_in_client_receiving_show_one_tasks()
        {
            converter.Convert("1").Returns(1);
            formatter1.CouldUse(1).Returns(true);
            list.Execute("1");
            client.Received().GetTaskById(1);
        }
    }

}
