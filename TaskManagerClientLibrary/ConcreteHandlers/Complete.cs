using System;
using System.IO;
using ConnectToWcf;
using NSubstitute;
using TaskManagerClientLibrary;
using TaskManagerClientLibrary.ConcreteHandlers;
using TaskManagerServiceLibrary;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Complete : Command<int>
    {
        public Complete(IClientConnection client, ArgumentConverter<int> converter, TextWriter textWriter)
            : base(client, converter, textWriter)
        {
        }

        protected override void ExecuteWithGenericInput(int input)
        {
            client.Complete(input);
            OutText(string.Format("Task ID: {0} completed.", input));
        }



        public class CompleteTests
        {
            private readonly ArgumentConverter<int> converter = Substitute.For<ArgumentConverter<int>>();
            private readonly IClientConnection client = Substitute.For<IClientConnection>();
            private readonly Complete handler;

            public CompleteTests()
            {
                handler = new Complete(client, converter, new StringWriter());
            }

            [Fact]
            public void should_send_string_return_id()
            {
                converter.Convert("5").Returns(5);
                handler.Execute("5");
                client.Received().Complete(5);
            }
        }
    }
}
