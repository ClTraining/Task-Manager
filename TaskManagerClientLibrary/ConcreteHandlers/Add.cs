using System;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Add : Command<string>
    {
        public Add(IClientConnection client) : base(client, typeof(Add)) { }

        protected override void Execute(string input)
        {
            var result = client.AddTask(input);
            Console.WriteLine("Task added. Task ID: " + result);
        }
    }

    public class AddTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly Add handler;
        const string taskName = "sometask1";

        public AddTests()
        {
            handler = new Add(client);
        }

        [Fact]
        public void should_send_string_return_id()
        {
            handler.Execute(taskName);
            client.Received().AddTask("sometask1");
        }

        [Fact]
        public void should_convert_to_string()
        {
            var result = handler.Convert("dhgfdhg");
            result.Should().Be("dhgfdhg");
        }
    }
}