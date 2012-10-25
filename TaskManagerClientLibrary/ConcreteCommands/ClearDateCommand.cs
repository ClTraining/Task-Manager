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
    public class ClearDateCommand : ICommand
    {
        private readonly IClient client;
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }
        private readonly ArgumentConverter<ClearDateTaskArgs> converter;
        private readonly TextWriter textWriter;

        public ClearDateCommand(ArgumentConverter<ClearDateTaskArgs> converter, TextWriter textWriter, IClient client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Clears due date for specified task by ID.";
        }

        public void Execute(List<string> argument)
        {
            var clearDateArgs = converter.Convert(argument);
            client.ClearTaskDueDate(clearDateArgs);
            textWriter.WriteLine(string.Format("Due date cleared for task ID: {0} .", clearDateArgs.Id));
        }
    }

    public class ClearDateTests
    {
        private readonly IClient client = Substitute.For<IClient>();
        private readonly ArgumentConverter<ClearDateTaskArgs> converter = Substitute.For<ArgumentConverter<ClearDateTaskArgs>>();
        private readonly ClearDateCommand handler;

        public ClearDateTests()
        {
            handler = new ClearDateCommand(converter, new StringWriter(), client);
        }

        [Fact]
        public void property_name_should_be_classname()
        {
            handler.Name.Should().Be("cleardate");
        }

        [Fact]
        public void should_send_string_return_id()
        {
            var arguments = new List<string> { "12" };
            var clearDateArgs = new ClearDateTaskArgs { Id = 12 };
            converter.Convert(arguments).Returns(clearDateArgs);
            handler.Execute(arguments);
            client.Received().ClearTaskDueDate(clearDateArgs);
        }
    }
}