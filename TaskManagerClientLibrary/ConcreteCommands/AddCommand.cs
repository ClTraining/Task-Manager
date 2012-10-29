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
    public class AddCommand : ICommand
    {
        private readonly TaskArgsConverter<AddTaskArgs> converter;
        private readonly TextWriter textWriter;
        private readonly IClient client;
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }

        public AddCommand(TaskArgsConverter<AddTaskArgs> converter, TextWriter textWriter, IClient client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Adds new task to server.";
        }

        public void Execute(List<string> argument)
        {
            var addTaskArgs = ConvertToArgs(argument);
            var result = client.AddTask(addTaskArgs);
            Printinfo(result);
        }

        private void Printinfo(int result)
        {
            textWriter.WriteLine("Task added. Task ID: " + result);
        }

        private AddTaskArgs ConvertToArgs(List<string> argument)
        {
            var addTaskArgs = converter.Convert(argument);
            return addTaskArgs;
        }
    }

    public class AddTests
    {
        private const string taskName = "sometask1";
        private readonly IClient client = Substitute.For<IClient>();
        private readonly TaskArgsConverter<AddTaskArgs> converter = Substitute.For<TaskArgsConverter<AddTaskArgs>>();
        private readonly AddCommand handler;

        public AddTests()
        {
            handler = new AddCommand(converter, new StringWriter(), client);
        }

        [Fact]
        public void property_name_should_return_class_name()
        {
            handler.Name.Should().Be("add");
        }

        [Fact]
        public void should_execute_on_client_add_task()
        {
            var addTaskArgs = new AddTaskArgs { Name = taskName };
            var argument = new List<string> { taskName };
            converter.Convert(argument).Returns(addTaskArgs);
            handler.Execute(argument);
            client.Received().AddTask(addTaskArgs);
        }
    }
}