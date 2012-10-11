using System;
using System.Collections.Generic;
using System.Linq;
using ConnectToWcf;
using EntitiesLibrary;
using NSubstitute;
using TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class List : Command<string>
    {
        private readonly List<ITaskFormatter> formatter;

        public List(IClientConnection client, ArgumentConverter<string> converter, List<ITaskFormatter> formatter) : base(client, typeof(List), converter)
        {
            this.formatter = formatter;
        }

        public override void ExecuteWithGenericInput(string input)
        {
            var tasks = string.IsNullOrEmpty(input)
                            ? client.GetAllTasks()
                            : client.GetTaskById(int.Parse(input));
            Show(tasks);
        }

        public void Show(List<ContractTask> tasks)
        {
            formatter.First(a => a.CountRange.Contains(tasks.Count)).Show(tasks);
        }
    }

    public class ListTester
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly List<ITaskFormatter> formatter = Substitute.For<List<ITaskFormatter>>();
        private readonly List list;

        public ListTester()
        {
            list = new List(client,new ArgumentConverter<string>(), formatter);
        }

        [Fact]
        public void should_check_receiving_one_task()
        {
            var taskList = new List<ContractTask> {new ContractTask {Id = 1, Name = "some", IsCompleted = false}};
            client.GetTaskById(1).Returns(taskList);
            list.ExecuteWithGenericInput("1");
            list.Show(taskList);
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
            list.ExecuteWithGenericInput("");
        }
    }
}
