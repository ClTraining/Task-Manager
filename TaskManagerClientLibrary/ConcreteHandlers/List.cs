using System;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class List : Command<string>
    {
        public List(IClientConnection client, ArgumentConverter<string> converter) : base(client, typeof(List), converter) { }

        protected override void ExecuteWithGenericInput(string input)
        {
            var tasks = string.IsNullOrEmpty(input) 
                ? client.GetAllTasks() 
                : client.GetTaskById(int.Parse(input));

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
        public void list_name_should_be_the_same_as_class_name()
        {
            list.Name.Should().BeEquivalentTo("list");
        }
    }
}
