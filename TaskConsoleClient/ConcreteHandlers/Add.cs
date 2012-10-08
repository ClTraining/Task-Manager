using System;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerConsole.ConcreteHandlers
{
    public class Add : Command<string>
    {
        private readonly IClientConnection manager;
        public Add(IClientConnection manager)
        {
            this.manager = manager;
            Name = typeof(Add).Name.ToLower();
        }

        protected override void Execute(string input)
        {
            var resultId = manager.AddTask(input);
            Console.WriteLine("Task added. Task ID: " + resultId);
        }
    }

    public class AddTests
    {
        private readonly IClientConnection manager = Substitute.For<IClientConnection>();
        private readonly Add handler;
        const string TaskName = "sometask1";

        public AddTests()
        {
            handler = new Add(manager);
        }

        [Fact]
        public void should_send_string_return_id()
        {
            handler.Execute(TaskName);
            manager.Received().AddTask("sometask1");
        }

        [Fact]
        public void should_convert_to_string()
        {
            var result = handler.Convert("dhgfdhg");
            result.Should().Be("dhgfdhg");
        }
    }
}