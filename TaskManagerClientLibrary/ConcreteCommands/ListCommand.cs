using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IClient client;
        private readonly TaskArgsConverter converter;
        private readonly ITaskFormatterFactory factory;
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

        public string Name
        {
            get { return GetType().Name.Split(new[] {"Command"}, StringSplitOptions.None)[0].ToLower(); }
        }

        public string Description { get; private set; }

        public void Execute(List<string> argument)
        {
            var listArgs = GetClientSpecification(argument);
            var formatter = factory.GetFormatter(listArgs);

            var tasks = client.GetTasks(listArgs);
            PrintWithFormatter(tasks, formatter);
        }

        private IListCommandArguments GetClientSpecification(List<string> source)
        {
            var types = new List<Type>
                            {typeof (ListByDateTaskArgs), typeof (ListSingleTaskArgs), typeof (ListAllTaskArgs)};

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
        private readonly ListCommand command;
        private readonly TaskArgsConverter converter = Substitute.For<TaskArgsConverter>();
        private readonly ITaskFormatterFactory factory = Substitute.For<ITaskFormatterFactory>();
        private readonly StringBuilder sb = new StringBuilder();
        private readonly StringWriter sw;
        private IListCommandArguments args;
        private readonly List<Type> types = new List<Type> { typeof(ListByDateTaskArgs), typeof(ListSingleTaskArgs), typeof(ListAllTaskArgs) };

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
        public void should_get_list_all_arguments()
        {
            args = new ListAllTaskArgs();
            var argument = new List<string> {"153"};
            converter.Convert(argument, Arg.Is<List<Type>>(listTypes => types.SequenceEqual(types))).Returns(args);
            client.GetTasks(args).Returns(new List<ClientTask>());

            command.Execute(argument);
            client.Received().GetTasks(args);
        }

        [Fact]
        public void should_get_list_single_arguments()
        {
            args = new ListSingleTaskArgs();
            var argument = new List<string>();
            converter.Convert(argument, Arg.Is<List<Type>>(listTypes => types.SequenceEqual(types))).Returns(args);

            command.Execute(argument);
            client.Received().GetTasks(args);
        }

        [Fact]
        public void should_get_list_by_date_arguments()
        {
            args = new ListByDateTaskArgs();
            var argument = new List<string>();
            converter.Convert(argument, Arg.Is<List<Type>>(listTypes => types.SequenceEqual(types))).Returns(args);

            command.Execute(argument);
            client.Received().GetTasks(args);
        }

        [Fact]
        public void should_print_info_on_required_tasks()
        {
            var argument = new List<string> {"153"};
            var listPackage = new List<ClientTask> {new ClientTask {DueDate = DateTime.Now, Id = 1, IsCompleted = true}};
            var formatter = Substitute.For<ITaskFormatter>();

            converter.Convert(argument, Arg.Is<List<Type>>(listTypes => types.SequenceEqual(types))).Returns(args);
            factory.GetFormatter(args).Returns(formatter);
            formatter.ToFormatString(listPackage).Returns("hello world");
            client.GetTasks(args).Returns(listPackage);

            command.Execute(argument);
            sb.ToString().Should().BeEquivalentTo("hello world\r\n");
        }
    }
}