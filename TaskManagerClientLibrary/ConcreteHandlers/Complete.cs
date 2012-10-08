using System;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Complete : Command<int>
    {
        public Complete(IClientConnection client) : base (client, typeof(Complete)) { }

        protected override void Execute(int input)
        {
            client.Complete(input);
            Console.WriteLine("Task ID: {0} completed.", input);
        }

    }
    public class ConcreteHandlerCompleteTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly Complete handler;

        public ConcreteHandlerCompleteTests()
        {
            handler = new Complete(client);
        }

        [Fact]
        public void should_convert_to_int()
        {
            var result = handler.Convert("222");
            result.Should().Be(222);
        }
    }
}
