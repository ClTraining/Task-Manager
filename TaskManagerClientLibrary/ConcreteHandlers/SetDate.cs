using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class SetDate : ICommand
    {
        private readonly ArgumentConverter<SetDateArgs> converter;
        private readonly TextWriter textWriter;
        private readonly IClientConnection client;
        public string Name { get { return GetType().Name.ToLower(); } }
        public string Description { get; private set; }

        public SetDate(ArgumentConverter<SetDateArgs> converter, TextWriter textWriter, IClientConnection client)
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

        private void PrintInfo(SetDateArgs setDateArgs)
        {
            textWriter.WriteLine("Due date to task assigned. Task Id:" + setDateArgs.Id);
        }

        private SetDateArgs ConvertToArgs(List<string> argument)
        {
            var setDateArgs = converter.Convert(argument);
            return setDateArgs;
        }
    }

    public class SetDateTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ICommand command;
        private readonly ArgumentConverter<SetDateArgs> converter = Substitute.For<ArgumentConverter<SetDateArgs>>();
        private readonly TextWriter writer = Substitute.For<TextWriter>();


        public SetDateTests()
        {
            command = new SetDate(converter, writer, client);
        }

        [Fact]
        public void should_send_set_date_to_client()
        {
            var setDateArgs = new SetDateArgs { Id = 5, DueDate = DateTime.Parse("10-10-2012") };
            var argument = new List<string> { "1", "10-10-2012" };
            converter.Convert(argument).Returns(setDateArgs);
            command.Execute(argument);
            client.Received().SetTaskDueDate(setDateArgs);
        }
    }
}