using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Xunit;
using System.Linq;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public class ClearDateCommand : ICommand
    {
        private readonly IClient client;
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }
        private readonly TaskArgsConverter converter;
        private readonly TextWriter textWriter;

        public ClearDateCommand(TaskArgsConverter converter, TextWriter textWriter, IClient client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Clears due date for specified task by ID.";
        }

        public void Execute(List<string> argument)
        {
            var clearDateArgs =
                converter.Convert(argument, new List<Type> {typeof (ClearDateTaskArgs)}) as ClearDateTaskArgs;

            client.ExecuteCommand(clearDateArgs);
            if (clearDateArgs != null) textWriter.WriteLine("Due date cleared for task ID: {0} .", clearDateArgs.Id);
        }
    }

    public class ClearDateTests
    {
        private readonly IClient client = Substitute.For<IClient>();
        private readonly TaskArgsConverter converter = Substitute.For<TaskArgsConverter>();
        private readonly ClearDateCommand command;
        readonly ClearDateTaskArgs args = new ClearDateTaskArgs { Id = 12 };
        readonly List<string> argument = new List<string> { "12" };

        public ClearDateTests()
        {
            command = new ClearDateCommand(converter, new StringWriter(), client);
            converter
                .Convert(argument, Arg.Is<List<Type>>(list => list.SequenceEqual(new List<Type> {typeof (ClearDateTaskArgs)})))
                .Returns(args);
        }

        [Fact]
        public void property_name_should_be_classname()
        {
            command.Name.Should().Be("cleardate");
        }

        [Fact]
        public void should_send_string_return_id()
        {
            command.Execute(argument);
            client.Received().ExecuteCommand(args);
        }

    }
}