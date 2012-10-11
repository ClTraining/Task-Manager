using System;
using System.Collections.Generic;
using ConnectToWcf;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary;
using Xunit;
using System.Linq;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class List : Command<string>
    {

        public List(IClientConnection client, ArgumentConverter<string> converter) : base(client, typeof(List), converter) { }

        protected override void ExecuteWithGenericInput(string input)
        {
            List<ContractTask> tasks = null;
            try
            {
                tasks = (string.IsNullOrEmpty(input))
                            ? client.GetAllTasks()
                            : client.GetTaskById(int.Parse(input));
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
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly List list;

        public ListTests()
        {
            list = new List(client, new ArgumentConverter<string>());
        }

        [Fact]
        public void list_name_should_be_the_same_as_class_name()
        {
            list.Name.Should().BeEquivalentTo("list");
        }
    }
}
