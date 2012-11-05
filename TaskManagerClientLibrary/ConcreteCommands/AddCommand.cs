using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public class AddCommand : ICommand
    {
        private readonly TaskArgsConverter converter;
        private readonly TextWriter textWriter;
        private readonly IClient client;
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }

        public AddCommand(TaskArgsConverter converter, TextWriter textWriter, IClient client)
        {
            this.converter = converter;
            this.textWriter = textWriter;
            this.client = client;
            Description = "Adds new task to server.";
        }

        public void Execute(List<string> argument)
        {
            var addTaskArgs = ConvertToArgs(argument);

            try
            {
                var result = client.AddTask(addTaskArgs);
                PrintInfo(result);
            }
            catch (ServerNotAvailableException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private void PrintInfo(int result)
        {
            textWriter.WriteLine("Task added. Task ID: " + result);
        }

        private AddTaskArgs ConvertToArgs(List<string> argument)
        {
            var addTaskArgs = converter.Convert(argument, new List<Type>{typeof(AddTaskArgs)}) as AddTaskArgs;
            return addTaskArgs;
        }
    }

    public class AddTests
    {
        private const string taskName = "sometask1";
        private readonly IClient client = Substitute.For<IClient>();
        private readonly TaskArgsConverter converter = Substitute.For<TaskArgsConverter>();
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
            var args = new AddTaskArgs { Name = taskName };
            var argument = new List<string> { taskName };

            converter
                .Convert(argument, Arg.Is<List<Type>>(list => list.SequenceEqual(new List<Type> { typeof(AddTaskArgs) })))
                .Returns(args);
            handler.Execute(argument);
            client.Received().AddTask(args);
        }

        [Fact]
        public void should_throw_exception_if_server_doesnt_response()
        {
            var args = new AddTaskArgs{Name = taskName};
        }
    }
}