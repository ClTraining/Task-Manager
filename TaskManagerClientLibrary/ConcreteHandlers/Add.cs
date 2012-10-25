using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Add : ICommand
    {
        private readonly ArgumentConverter<AddTaskArgs> converter;
        private readonly TextWriter textWriter;
        private readonly IClientConnection client;
        public string Name { get { return GetType().Name.ToLower(); } }
        public string Description { get; private set; }

        public Add(ArgumentConverter<AddTaskArgs> converter, TextWriter textWriter, IClientConnection client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Adds new task to server.";
        }

        public void Execute(List<string> argument)
        {
            var addTaskArgs = ConvertToArgs(argument);
            var result = client.AddTask(addTaskArgs);
            Printinfo(result);
        }

        private void Printinfo(int result)
        {
            textWriter.WriteLine("Task added. Task ID: " + result);
        }

        private AddTaskArgs ConvertToArgs(List<string> argument)
        {
            var addTaskArgs = converter.Convert(argument);
            return addTaskArgs;
        }
    }

    public class AddTests
    {
        private const string taskName = "sometask1";
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ArgumentConverter<AddTaskArgs> converter = Substitute.For<ArgumentConverter<AddTaskArgs>>();
        private readonly Add handler;

        public AddTests()
        {
            handler = new Add(converter, new StringWriter(), client);
        }

        [Fact]
        public void should_execute_on_client_add_task()
        {
            var addTaskArgs = new AddTaskArgs { Name = taskName };
            var argument = new List<string> { taskName };
            converter.Convert(argument).Returns(addTaskArgs);
            handler.Execute(argument);
            client.Received().AddTask(addTaskArgs);
        }
    }
}