using System.IO;
using ConnectToWcf;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Add : Command<string>
    {
        public Add(IClientConnection client, ArgumentConverter<string> converter, TextWriter textWriter) : base(client, converter, textWriter)
        {
        }

        protected override void ExecuteWithGenericInput(string input)
        {
            var result = client.AddTask(input);
            OutText("Task added. Task ID: " + result);
        }
    }

    public class AddTests
    {
        private readonly ArgumentConverter<string> converter = Substitute.For<ArgumentConverter<string>>(); 
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly Add handler;
        const string taskName = "sometask1";

        public AddTests()
        {
            handler = new Add(client, converter, new StringWriter());
        }

        [Fact]
        public void should_execute_on_client_add_task()
        {
            handler.Execute(taskName);
            client.Received().AddTask("sometask1");
        }
    }
}