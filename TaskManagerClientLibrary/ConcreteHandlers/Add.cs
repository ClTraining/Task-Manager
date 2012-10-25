using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Add : Command<AddTaskArgs>
    {
        private IClientConnection Client { get; set; }

        public Add(ArgumentConverter<AddTaskArgs> converter, TextWriter textWriter, IClientConnection client)
            : base(converter, textWriter)
        {
            Client = client;
            Description = "Adds new task to server.";
        }

        public override void Execute(List<string> argument)
        {
            var addTaskArgs = converter.Convert(argument);
            var result = Client.AddTask(addTaskArgs);
            OutText("Task added. Task ID: " + result);
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