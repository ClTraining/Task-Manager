using System;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Complete : Command<int>
    {
        private readonly IClientConnection manager;

        public Complete(IClientConnection manager)
        {
            this.manager = manager;
            Name = typeof(Complete).Name.ToLower();
        }

        protected override void Execute(int input)
        {
            manager.MarkCompleted(input);
            Console.WriteLine("Task ID: {0} completed.", input);
        }

    }
    public class ConcreteHandlerMarkCompletedTests
    {
        private readonly IClientConnection manager = Substitute.For<IClientConnection>();
        private readonly Complete handler;

        public ConcreteHandlerMarkCompletedTests()
        {
            handler = new Complete(manager);
        }

        [Fact]
        public void should_convert_to_int()
        {
            var result = handler.Convert("222");
            result.Should().Be(222);
        }
    }

}
