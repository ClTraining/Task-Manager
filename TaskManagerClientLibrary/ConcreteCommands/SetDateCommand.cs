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
    public class SetDateCommand : ICommand
    {
        private readonly ArgumentConverter<SetDateTaskArgs> converter;
        private readonly TextWriter textWriter;
        private readonly IClient client;
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }

        public SetDateCommand(ArgumentConverter<SetDateTaskArgs> converter, TextWriter textWriter, IClient client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Sets due date for task, specified by ID.";
        }

        public void Execute(List<string> argument)
        {
            var setDateArgs = ConvertToArgs(argument);
            client.SetTaskDueDate(setDateArgs);
            PrintInfo(setDateArgs);
        }

        private void PrintInfo(SetDateTaskArgs setDateArgs)
        {
            textWriter.WriteLine("Due date to task assigned. Task Id:" + setDateArgs.Id);
        }

        private SetDateTaskArgs ConvertToArgs(List<string> argument)
        {
            var setDateArgs = converter.Convert(argument);
            return setDateArgs;
        }
    }

    public class SetDateTests
    {
        private readonly IClient client = Substitute.For<IClient>();
        private readonly ICommand command;
        private readonly ArgumentConverter<SetDateTaskArgs> converter = Substitute.For<ArgumentConverter<SetDateTaskArgs>>();
        private readonly TextWriter writer = Substitute.For<TextWriter>();


        public SetDateTests()
        {
            command = new SetDateCommand(converter, writer, client);
        }

        [Fact]
        public void property_name_should_be_classname()
        {
            command.Name.Should().Be("setdate");
        }

        [Fact]
        public void should_send_set_date_to_client()
        {
            var setDateArgs = new SetDateTaskArgs { Id = 5, DueDate = DateTime.Parse("10-10-2012") };
            var argument = new List<string> { "1", "10-10-2012" };
            converter.Convert(argument).Returns(setDateArgs);
            command.Execute(argument);
            client.Received().SetTaskDueDate(setDateArgs);
        }
    }
}