using System;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Add : Command<string>
    {
        public Add(IClientConnection client, ArgumentConverter<string> converter) : base(client, typeof(Add), converter) { }

        public override void ExecuteWithGenericInput(string input)
        {
            try
            {
                var result = client.AddTask(input);
                Console.WriteLine("Task added. Task ID: " + result);
            }
            catch (TaskNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
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
            handler = new Add(client, converter);
        }

        [Fact]
        public void should_send_string_return_id()
        {
            converter.Convert(taskName).Returns(taskName);
            handler.Execute(taskName);
            client.Received().AddTask("sometask1");
        }
    }
}