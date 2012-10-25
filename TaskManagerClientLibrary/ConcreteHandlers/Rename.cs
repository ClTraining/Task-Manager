using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Rename : ICommand
    {
        private readonly IClientConnection client;
        public string Name { get { return GetType().Name.ToLower(); } }
        public string Description { get; private set; }
        private readonly ArgumentConverter<RenameTaskArgs> converter;
        private readonly TextWriter textWriter;

        public Rename(ArgumentConverter<RenameTaskArgs> converter, TextWriter textWriter, IClientConnection client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Renames task, specified by ID.";
        }

        public void Execute(List<string> argument)
        {
            var renameTaskArgs = ConvertToArgs(argument);
            client.RenameTask(renameTaskArgs);
            PrintInfo(renameTaskArgs);
        }

        private void PrintInfo(RenameTaskArgs renameTaskArgs)
        {
            textWriter.WriteLine(string.Format("Task ID: {0} renamed.", renameTaskArgs.Id));
        }

        private RenameTaskArgs ConvertToArgs(List<string> argument)
        {
            var renameTaskArgs = converter.Convert(argument);
            return renameTaskArgs;
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