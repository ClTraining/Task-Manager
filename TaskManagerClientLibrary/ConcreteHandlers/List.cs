using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConnectToWcf;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary;
using Xunit;
using System.Linq;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class List : Command<int?>
    {

        public List(IClientConnection client, ArgumentConverter<int?> converter) : base(client, typeof(List), converter) { }

        protected override void ExecuteWithGenericInput(int? input)
        {
            List<ContractTask> tasks;
            try
            {
                tasks = (input == null)
                            ? client.GetAllTasks()
                            : client.GetTaskById(input.Value);
            }
            catch (TaskNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var delim = tasks.Count > 1 ? '\t' : '\n';

            if (tasks.Count == 0)
            {
                Console.WriteLine("Task list is empty");
            }
            else if (tasks.Count == 1)
            {
                Console.WriteLine();
                Console.WriteLine("ID: {0}" + delim + "Name: {1}" + delim + "Completed: {2}", tasks[0].Id, tasks[0].Name,
                                  tasks[0].IsCompleted ? "+" : "-");
            }
            else if (tasks.Count > 1)
            {
                Console.WriteLine(" ID\t|\tName\t\t|\tCompleted");
                tasks.ForEach(x => Console.WriteLine(" {0}\t|" + delim + "{1}" + delim + "\t|\t{2}", x.Id, x.Name, x.IsCompleted ? "+" : "-"));
            }

        }
    }

    public class ListTests
    {
        private readonly ArgumentConverter<int?> converter = Substitute.For<ArgumentConverter<int?>>();
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly List handler;

        public ListTests()
        {
            handler = new List(client, converter);
        }

        [Fact]
        public void list_name_should_be_the_same_as_class_name()
        {
            handler.Name.Should().BeEquivalentTo("list");
        }

        [Fact]
        public void should_send_string_return_id()
        {
            const string id = "5";
            client.GetTaskById(5).Returns(new List<ContractTask>());

            converter.Convert(id).Returns(int.Parse(id));
            handler.Execute(id);

            client.Received().GetTaskById(5);
        }

        [Fact]
        public void if_input_is_null_should_get_all_tasks()
        {
            client.GetAllTasks().Returns(new List<ContractTask> { new ContractTask(), new ContractTask() });

            converter.Convert(null).Returns(null as int?);
            handler.Execute(null);

            client.Received().GetAllTasks();
        }

        [Fact]
        public void if_get_task_by_id_should_call_specifical_printer_for_it()
        {
            const string id = "5";
            client.GetTaskById(5).Returns(new List<ContractTask> { new ContractTask() });

            converter.Convert(id).Returns(int.Parse(id));
            handler.Execute(id);

            client.Received().GetTaskById(5);
        }

        [Fact]
        public void should_inform_user_about_exceptions()
        {
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            client.GetTaskById(5).Returns(x => { throw new TaskNotFoundException(5); });

            converter.Convert("5").Returns(5);
            handler.Execute("5");

            sb.ToString().ShouldBeEquivalentTo("Task not found: (Id = 5)\r\n");
        }
    }
}
