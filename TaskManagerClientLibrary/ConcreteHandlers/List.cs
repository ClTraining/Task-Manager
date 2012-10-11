using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConnectToWcf;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class List : Command<string>
    {
        private List<ContractTask> tasks = new List<ContractTask>(); 

        public List(IClientConnection client, ArgumentConverter<string> converter) : base(client, typeof(List), converter) { }

        public override void ExecuteWithGenericInput(string input)
        {
            tasks = string.IsNullOrEmpty(input) 
                ? client.GetAllTasks() 
                : client.GetTaskById(int.Parse(input));

            if (tasks.Count == 1)
                ShowTask(tasks[0]);

            else ShowAllTasks();
        }

        public void ShowTask(ContractTask task)
        {
            Console.Write("\nID: {0}\nName: {1}\nCompleted: {2}\n", task.Id, task.Name, task.IsCompleted ? "+" : "-");
        }

        public void ShowAllTasks()
        {
            tasks.ForEach(x => Console.WriteLine("ID: {0}\tTask: {1}\t\t\tCompleted: {2}", x.Id, x.Name, x.IsCompleted ? "+" : "-"));            
        }
    }

    public class ListTester
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly List list;

        public ListTester()
        {
            list = new List(client,new ArgumentConverter<string>());
        }

        [Fact]
        public void should_display_single_task()
        {
            var sb = new StringBuilder();
            var task = new ContractTask {Id = 2, Name = "some task", IsCompleted = false};

            Console.SetOut(new StringWriter(sb));
            list.ShowTask(task);
            
            sb.ToString().Should().Be("\nID: 2\n" +
                                      "Name: some task\n" +
                                      "Completed: -\n");
        }

        [Fact]
        public void should_check_receiving_one_task()
        {
            var taskList = new List<ContractTask> {new ContractTask {Id = 1, Name = "some", IsCompleted = false}};
            client.GetTaskById(1).Returns(taskList);
            list.ExecuteWithGenericInput("1");
            list.ShowTask(taskList[0]);
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
            list.ShowAllTasks();
        }
    }
}
