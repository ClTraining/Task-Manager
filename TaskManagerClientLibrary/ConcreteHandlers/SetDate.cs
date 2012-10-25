using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class SetDate : Command<SetDateArgs>
    {
        private IClientConnection Client { get; set; }

        public SetDate(ArgumentConverter<SetDateArgs> converter, TextWriter textWriter, IClientConnection client)
            : base(converter, textWriter)
        {
            Client = client;
            Description = "Sets due date for task, specified by ID.";
        }

        public override void Execute(List<string> argument)
        {
            var setDateArgs = converter.Convert(argument);
            Client.SetTaskDueDate(setDateArgs);
            OutText(string.Format("Due date to task assigned. Task Id:{0}", setDateArgs.Id));
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