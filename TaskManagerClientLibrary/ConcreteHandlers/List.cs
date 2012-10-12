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
    public class List : Command<string>
    {
        private readonly Func<string, ITaskFormatter> getFormatter;

        public List(IClientConnection client, ArgumentConverter<string> converter, TextWriter textWriter, Func<string, ITaskFormatter> formatter)
            : base(client,  converter, textWriter)
        {
            getFormatter = formatter;
        }

        protected override void ExecuteWithGenericInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                ExecutePr(s => s.GetAllTasks(), getFormatter(input));
            else
                ExecutePr(s => s.GetTaskById(int.Parse(input)), getFormatter(input));
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
        private readonly ITaskFormatter formatter1 = Substitute.For<ITaskFormatter>();
        private readonly ITaskFormatter formatter2 = Substitute.For<ITaskFormatter>();
        private readonly List list;

        public ListTests()
        {

            list = new List(client, new ArgumentConverter<string>(), new StringWriter(), c => formatter1);
        }

        [Fact]
        public void should_check_receiving_one_task()
        {
            var taskList = new List<ContractTask> { new ContractTask { Id = 1, Name = "some", IsCompleted = false } };
            client.GetTaskById(1).Returns(taskList);
            list.Execute("1");
            formatter1.Received().Show(taskList);
        }

        [Fact]
        public void should_check_receiving_all_task()
        {
            var taskList = new List<ContractTask>
                               {
                                   new ContractTask { Id = 1, Name = "task1", IsCompleted = false },
                                   new ContractTask{Id = 2, Name = "task2", IsCompleted = true}
                               };
            client.GetAllTasks().Returns(taskList);
            list.Execute("");
            formatter1.Received().Show(taskList);
        }
    }

}
