using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Add : Command<AddTaskArgs>
    {
        public Add(IClientConnection client, ArgumentConverter<AddTaskArgs> converter, TextWriter textWriter)
            : base(client, converter, textWriter)
        {
            Description = "Adds new task to server.";
        }

        protected override void ExecuteWithGenericInput(AddTaskArgs input)
        {
            var result = client.AddTask(input);
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
            handler = new Add(client, converter, new StringWriter());
        }

        [Fact]
        public void should_execute_on_client_add_task()
        {
            var addTaskArgs = new AddTaskArgs {Name = taskName};
            var argument = new List<string> {taskName};
            converter.Convert(argument).Returns(addTaskArgs);
            handler.Execute(argument);
            client.Received().AddTask(addTaskArgs);
        }
    }
}