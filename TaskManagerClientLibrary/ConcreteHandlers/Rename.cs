using System;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary;
using NSubstitute;
using TaskManagerServiceLibrary;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Rename : Command<RenameTaskArgs>
    {
        public Rename(IClientConnection client, ArgumentConverter<RenameTaskArgs> converter, TextWriter textWriter)
            : base(client, converter, textWriter)
        {
        }

        protected override void ExecuteWithGenericInput(RenameTaskArgs input)
        {
            client.RenameTask(input);
            OutText(string.Format("Task ID: {0} renamed.", input.Id));
        }
    }

    public class RenameTests
    {
        private readonly ArgumentConverter<RenameTaskArgs> converter = Substitute.For<ArgumentConverter<RenameTaskArgs>>();
        private readonly TextWriter textWriter = Substitute.For<TextWriter>();
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly Rename renameCommand;
        public RenameTests()
        {
            renameCommand = new Rename(client, converter, textWriter);
        }

        [Fact]
        public void should_send_to_client_rename_task()
        {
            var renameTaskArgs = new RenameTaskArgs { Id = 1, Name = "taskName" };
            converter.Convert("1 taskName").Returns(renameTaskArgs);
            renameCommand.Execute("1 taskName");
            client.Received().RenameTask(renameTaskArgs);
        }
    }
}
