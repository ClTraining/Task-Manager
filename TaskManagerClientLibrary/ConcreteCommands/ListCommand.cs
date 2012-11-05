using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConnectToWcf;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using TaskManagerClientLibrary.ConcreteCommands.TaskFormatter;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public class ListCommand : ICommand
    {
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }
        private readonly IClient client;
        private readonly ITaskFormatterFactory factory;
        private readonly TaskArgsConverter converter;
        private readonly TextWriter textWriter;

        public ListCommand(TaskArgsConverter converter, TextWriter textWriter,
                    IClient client, ITaskFormatterFactory factory)
        {
            Description = "Displays list of all tasks or single task, specified by ID.";
            this.converter = converter;
            this.textWriter = textWriter;

            this.client = client;
            this.factory = factory;
        }

        public void Execute(List<string> argument)
        {
            var listArgs = GetClientSpecification(argument);
            var formatter = factory.GetFormatter(listArgs);

            try
            {
                var tasks = client.GetTasks(listArgs);
                PrintWithFormatter(tasks, formatter);
            }
            catch (ServerNotAvailableException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private IListCommandArguments GetClientSpecification(List<string> source)
        {
            var types = new List<Type> { typeof(ListByDateTaskArgs), typeof(ListSingleTaskArgs), typeof(ListAllTaskArgs) };

            return converter.Convert(source, types) as IListCommandArguments;
        }

        private void PrintWithFormatter(List<ClientTask> list, ITaskFormatter formatter)
        {
            textWriter.WriteLine(formatter.ToFormatString(list));
        }
    }

    public class ListTests
    {
        private readonly IClient client = Substitute.For<IClient>();
        private readonly TaskArgsConverter converter = Substitute.For<TaskArgsConverter>();
        private IListCommandArguments args;
        private readonly StringBuilder sb = new StringBuilder();
        readonly StringWriter sw;
        private readonly ListCommand command;
        private readonly ITaskFormatterFactory factory = Substitute.For<ITaskFormatterFactory>();

        public ListTests()
        {
            sw = new StringWriter(sb);
            command = new ListCommand(converter, sw, client, factory);
        }

        [Fact]
        public void property_name_should_be_classname()
        {
            command.Name.Should().Be("list");
        }

        [Fact]
        public void should_get_all_commands()
        {
            args = new ListAllTaskArgs();
            var argument = new List<string> { "153" };
            converter.Convert(argument, new List<Type> { typeof(ListAllTaskArgs) }).Returns(args);
            client.GetTasks(args).Returns(new List<ClientTask>());

            command.Execute(argument);
            client.ReceivedWithAnyArgs().GetTasks(args);
        }
        [Fact]
        public void should_get_one_command_by_id()
        {
            args = new ListSingleTaskArgs();
            var argument = new List<string>();
            client.GetTasks(args).Returns(new List<ClientTask>());
            converter.Convert(argument, new List<Type> { typeof(ListSingleTaskArgs) }).Returns(new ListSingleTaskArgs());

            command.Execute(argument);
            client.ReceivedWithAnyArgs().GetTasks(args);
        }
        [Fact]
        public void should_get_one_command_by_date()
        {
            args = new ListByDateTaskArgs();
            var argument = new List<string>();
            client.GetTasks(args).Returns(new List<ClientTask>());
            converter.Convert(argument, new List<Type> { typeof(ListByDateTaskArgs) }).Returns(args);

            command.Execute(argument);
            client.ReceivedWithAnyArgs().GetTasks(args);
        }

        [Fact]
        public void should_throw_exception_if_server_is_not_available()
        {
            var argument = new List<string> { "153" };
            var listPackage = new List<ClientTask> { new ClientTask { DueDate = DateTime.Now, Id = 1, IsCompleted = true } };
            var formatter = Substitute.For<ITaskFormatter>();
            var types = new List<Type> { typeof(ListByDateTaskArgs), typeof(ListSingleTaskArgs), typeof(ListAllTaskArgs) };

            converter.Convert(argument, types).Returns(args);
            factory.GetFormatter(args).Returns(formatter);
            formatter.ToFormatString(listPackage).Returns("hello world");

            client.When(c => c.GetTasks(args)).Do(_ => { throw new ServerNotAvailableException(); });
            Console.SetOut(new StringWriter(sb));

            command.Execute(argument);

            sb.ToString().Should().Be("Server is not available.\r\n");
        }

        [Fact]
        public void should_print_info_on_required_tasks()
        {
            var argument = new List<string> { "153" };
            var listPackage = new List<ClientTask> { new ClientTask { DueDate = DateTime.Now, Id = 1, IsCompleted = true } };
            var formatter = Substitute.For<ITaskFormatter>();
            var types = new List<Type> { typeof(ListByDateTaskArgs), typeof(ListSingleTaskArgs), typeof(ListAllTaskArgs) };

            converter.Convert(argument, types).ReturnsForAnyArgs(args);
            factory.GetFormatter(args).Returns(formatter);
            formatter.ToFormatString(listPackage).Returns("hello world");
            client.GetTasks(args).Returns(listPackage);

            command.Execute(argument);
            sb.ToString().Should().BeEquivalentTo("hello world\r\n");
        }
    }
}