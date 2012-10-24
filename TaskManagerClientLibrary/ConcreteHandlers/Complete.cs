using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
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
            client.Complete(input);
            OutText(string.Format("Task ID: {0} completed.", input.Id));
        }
    }

    public class CompleteTests
    {
        private const string taskName = "sometask1";
        private readonly IClientConnection client = Substitute.For<IClientConnection>();

        private readonly ArgumentConverter<CompleteTaskArgs> converter =
            Substitute.For<ArgumentConverter<CompleteTaskArgs>>();

        private readonly Complete command;

        public CompleteTests()
        {
            command = new Complete(client, converter, new StringWriter());
        }

        [Fact]
        public void should_send_set_date_to_client()
        {
            var completeTaskArgs = new CompleteTaskArgs {Id = 1};
            var argument = new List<string> { "1", "10-10-2012" };
            converter.Convert(argument).Returns(completeTaskArgs);
            command.Execute(argument);
            client.Received().Complete(completeTaskArgs);
        }
    }
}
