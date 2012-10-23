﻿using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Rename : Command<RenameTaskArgs>
    {
        public Rename(IClientConnection client, ArgumentConverter<RenameTaskArgs> converter, TextWriter textWriter)
            : base(client, converter, textWriter)
        {
            Description = "Renames task, specified by ID.";
        }

        protected override void ExecuteWithGenericInput(RenameTaskArgs input)
        {
            client.RenameTask(input);
            OutText(string.Format("Task ID: {0} renamed.", input.Id));
        }
    }

    public class RenameTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();

        private readonly ArgumentConverter<RenameTaskArgs> converter =
            Substitute.For<ArgumentConverter<RenameTaskArgs>>();

        private readonly Rename renameCommand;
        private readonly TextWriter textWriter = Substitute.For<TextWriter>();

        public RenameTests()
        {
            renameCommand = new Rename(client, converter, textWriter);
        }

        [Fact]
        public void should_send_to_client_rename_task()
        {
            var renameTaskArgs = new RenameTaskArgs {Id = 1, Name = "taskName"};
            var argument = new List<string> {"1", "taskName"};
            converter.Convert(argument).Returns(renameTaskArgs);
            renameCommand.Execute(argument);
            client.Received().RenameTask(renameTaskArgs);
        }
    }
}