using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Rename : Command<RenameTaskArgs>
    {
        private IClientConnection Client { get; set; }

        public Rename(ArgumentConverter<RenameTaskArgs> converter, TextWriter textWriter, IClientConnection client)
            : base(converter, textWriter)
        {
            Client = client;
            Description = "Renames task, specified by ID.";
        }

        protected override void ExecuteWithGenericInput(RenameTaskArgs input)
        {
            Client.RenameTask(input);
            OutText(string.Format("Task ID: {0} renamed.", input.Id));
        }


    }

    public class RenameTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();

        private readonly ArgumentConverter<RenameTaskArgs> converter =
            Substitute.For<ArgumentConverter<RenameTaskArgs>>();

        private readonly Rename command;
        private readonly TextWriter textWriter = Substitute.For<TextWriter>();

        public RenameTests()
        {
            command = new Rename(converter, textWriter, client);
        }

        [Fact]
        public void should_send_rename_to_client()
        {
            var renameTaskArgs = new RenameTaskArgs { Id = 5, Name = "newTask" };
            var argument = new List<string> { "1", "10-10-2012" };
            converter.Convert(argument).Returns(renameTaskArgs);
            command.Execute(argument);
            client.Received().RenameTask(renameTaskArgs);
        }
    }
}