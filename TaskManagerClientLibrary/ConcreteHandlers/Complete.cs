using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Complete : ICommand
    {
        private readonly IClientConnection client;
        public string Name { get { return GetType().Name.ToLower(); } }
        public string Description { get; private set; }
        private readonly ArgumentConverter<CompleteTaskArgs> converter;
        private readonly TextWriter textWriter;

        public Complete(ArgumentConverter<CompleteTaskArgs> converter, TextWriter textWriter, IClientConnection client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Mark task by ID as completed.";
        }

        public void Execute(List<string> argument)
        {
            var completeTaskArgs = ConvertToArgs(argument);
            client.Complete(completeTaskArgs);
            PrintInfo(completeTaskArgs);
        }

        private void PrintInfo(CompleteTaskArgs completeTaskArgs)
        {
            textWriter.WriteLine(string.Format("Task ID: {0} completed.", completeTaskArgs.Id));
        }

        private CompleteTaskArgs ConvertToArgs(List<string> argument)
        {
            var completeTaskArgs = converter.Convert(argument);
            return completeTaskArgs;
        }
    }

    public class CompleteTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();

        private readonly ArgumentConverter<CompleteTaskArgs> converter =
            Substitute.For<ArgumentConverter<CompleteTaskArgs>>();

        private readonly Complete command;

        public CompleteTests()
        {
            command = new Complete(converter, new StringWriter(), client);
        }

        [Fact]
        public void should_send_set_date_to_client()
        {
            var completeTaskArgs = new CompleteTaskArgs { Id = 1 };
            var argument = new List<string> { "1", "10-10-2012" };
            converter.Convert(argument).Returns(completeTaskArgs);
            command.Execute(argument);
            client.Received().Complete(completeTaskArgs);
        }
    }
}
