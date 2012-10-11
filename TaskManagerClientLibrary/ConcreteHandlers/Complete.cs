using System;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Complete : Command<int>
    {
        public Complete(IClientConnection client, ArgumentConverter<int> converter)
            : base(client, typeof(Complete), converter) { }

        public override void ExecuteWithGenericInput(int input)
        {
            client.Complete(input);
            Console.WriteLine("Task ID: {0} completed.", input);
        }
        }

    public class CompleteTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly Complete handler;

        public CompleteTests()
        {
            handler = new Complete(client, new ArgumentConverter<int>());
        }

        [Fact]
        public void should_send_string_return_id()
        {
            handler.Execute("5");
            client.Received().Complete(5);
        }
    }
}
