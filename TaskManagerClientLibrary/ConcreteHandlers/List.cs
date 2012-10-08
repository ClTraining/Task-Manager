using System;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class List : Command<string>
    {
        public List(IClientConnection client) : base (typeof(List))
        {
            base.client = client;
        }

        protected override void Execute(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                client
                    .GetAllTasks()
                    .ForEach(x =>
                        Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", x.Id, x.Name, x.IsCompleted ? "+" : "-"));
            }
            else
            {
                var argument = int.Parse(input);
                var task = client.GetTaskById(argument);
                Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", task.Id, task.Name, task.IsCompleted ? "+" : "-");
            }
        }
    }

    public class ListTester
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly List list;

        public ListTester()
        {
            list = new List(client);
        }

        [Fact]
        public void list_name_should_be_the_same_as_class_name()
        {
            list.Name.Should().BeEquivalentTo("list");
        }
    }
}
