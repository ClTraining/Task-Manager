using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class SetDate : Command<SetDateArgs>
    {
        public SetDate(IClientConnection client, ArgumentConverter<SetDateArgs> converter, TextWriter textWriter)
            : base(client, converter, textWriter)
        {
            Description = "Sets due date for task, specified by ID.";
        }

        protected override void ExecuteWithGenericInput(SetDateArgs input)
        {
            client.SetTaskDueDate(input);
            OutText(string.Format("Due date to task assigned. Task Id:{0}", input.Id));
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
            command = new SetDate(client, converter, writer);
        }

        [Fact]
        public void should_send_set_date_to_client()
        {
            var setDateArgs = new SetDateArgs {Id = 5, DueDate = DateTime.Parse("10-10-2012")};
            var argument = new List<string> {"1", "10-10-2012"};
            converter.Convert(argument).Returns(setDateArgs);
            command.Execute(argument);
            client.Received().SetTaskDueDate(setDateArgs);
        }
    }
}