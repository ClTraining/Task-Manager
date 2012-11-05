using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Xunit;
using System.Linq;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public class RenameCommand : ICommand
    {
        private readonly IClient client;
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }
        private readonly TaskArgsConverter converter;
        private readonly TextWriter textWriter;

        public RenameCommand(TaskArgsConverter converter, TextWriter textWriter, IClient client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Renames task, specified by ID.";
        }

        public void Execute(List<string> argument)
        {
            var renameTaskArgs = ConvertToArgs(argument);
            client.ExecuteCommand(renameTaskArgs);
            PrintInfo(renameTaskArgs);
        }

        private void PrintInfo(RenameTaskArgs renameTaskArgs)
        {
            textWriter.WriteLine("Task ID: {0} renamed.", renameTaskArgs.Id);
        }

        private RenameTaskArgs ConvertToArgs(List<string> argument)
        {
            var renameTaskArgs = converter.Convert(argument, new List<Type>{typeof(RenameTaskArgs)}) as RenameTaskArgs;
            return renameTaskArgs;
        }
    }

    public class RenameTests
    {
        private readonly IClient client = Substitute.For<IClient>();
        private readonly TaskArgsConverter converter = Substitute.For<TaskArgsConverter>();
        private readonly RenameCommand command;
        private readonly TextWriter textWriter = Substitute.For<TextWriter>();
        readonly RenameTaskArgs args = new RenameTaskArgs { Id = 5, Name = "newTask" };
        readonly List<string> argument = new List<string> { "1", "10-10-2012" };

        public RenameTests()
        {
            command = new RenameCommand(converter, textWriter, client);
            converter
                .Convert(argument, Arg.Is<List<Type>>(list => list.SequenceEqual(new List<Type> {typeof (RenameTaskArgs)})))
                .Returns(args);
        }

        [Fact]
        public void property_name_should_be_classname()
        {
            command.Name.Should().Be("rename");
        }

        [Fact]
        public void should_send_rename_to_client()
        {
            command.Execute(argument);
            client.Received().ExecuteCommand(args);
        }
    }
}