using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public class CompleteCommand : ICommand
    {
        private readonly IClient client;
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }
        private readonly ArgumentConverter<CompleteTaskArgs> converter;
        private readonly TextWriter textWriter;

        public CompleteCommand(ArgumentConverter<CompleteTaskArgs> converter, TextWriter textWriter, IClient client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Mark task by ID as completed.";
        }

        public void Execute(List<string> argument)
        {
            var completeTaskArgs = ConvertToArgs(argument);
            client.UpdateChanges(completeTaskArgs);
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
        private readonly IClient client = Substitute.For<IClient>();

        private readonly ArgumentConverter<CompleteTaskArgs> converter =
            Substitute.For<ArgumentConverter<CompleteTaskArgs>>();

        private readonly CompleteCommand command;

        public CompleteTests()
        {
            command = new CompleteCommand(converter, new StringWriter(), client);
        }

        [Fact]
        public void property_name_should_be_classname()
        {
            command.Name.Should().Be("complete");
        }

        [Fact]
        public void should_send_set_date_to_client()
        {
            var completeTaskArgs = new CompleteTaskArgs { Id = 1 };
            var argument = new List<string> { "1", "10-10-2012" };
            converter.Convert(argument).Returns(completeTaskArgs);
            command.Execute(argument);
            client.Received().UpdateChanges(completeTaskArgs);
        }
    }
}
