using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Complete : Command<CompleteTaskArgs>
    {
        public Complete(IClientConnection client, ArgumentConverter<CompleteTaskArgs> converter, TextWriter textWriter)
            : base(client, converter, textWriter)
        {
            Description = "Mark task by ID as completed.";
        }

        protected override void ExecuteWithGenericInput(CompleteTaskArgs input)
        {
            client.MarkTaskAsCompleted(input);
            OutText(string.Format("Task ID: {0} completed.", input.Id));
        }
    }

    public class CompleteTests
    {
        private const string taskName = "sometask1";
        private readonly IClientConnection client = Substitute.For<IClientConnection>();

        private readonly ArgumentConverter<CompleteTaskArgs> converter =
            Substitute.For<ArgumentConverter<CompleteTaskArgs>>();

        private readonly Complete handler;

        public CompleteTests()
        {
            handler = new Complete(client, converter, new StringWriter());
        }

        [Fact]
        public void should_send_string_return_id()
        {
            var completeTaskArgs = new CompleteTaskArgs {Id = 5};
            var arguments = new List<string> {"5"};
            converter.Convert(arguments).Returns(completeTaskArgs);
            handler.Execute(arguments);
            client.Received().MarkTaskAsCompleted(completeTaskArgs);
        }
    }
}
