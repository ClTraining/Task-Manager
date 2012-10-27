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
using Specifications.ClientSpecifications;
using TaskManagerClientLibrary.ConcreteCommands.TaskFormatter;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public class ListCommand : ICommand
    {
        public string Name { get { return GetType().Name.Split(new[] { "Command" }, StringSplitOptions.None)[0].ToLower(); } }
        public string Description { get; private set; }
        private readonly IClient client;
        private readonly ArgumentConverter<ListTaskArgs> converter;
        private readonly TextWriter textWriter;
        private readonly ITaskFormatterFactory taskFormatterFactory;
        private readonly IClientSpecificatinsFactory specificatinsFactory;

        public ListCommand(ArgumentConverter<ListTaskArgs> converter, TextWriter textWriter,
                    ITaskFormatterFactory taskFormatterFactory, IClient client, IClientSpecificatinsFactory specificatinsFactory)
        {
            Description = "Displays list of all tasks or single task, specified by ID.";
            this.converter = converter;
            this.textWriter = textWriter;
            this.taskFormatterFactory = taskFormatterFactory;
            this.client = client;
            this.specificatinsFactory = specificatinsFactory;
        }

        private void PrintWithFormatter(List<ClientPackage> list, ITaskFormatter formatter)
        {
            textWriter.WriteLine(formatter.ToFormatString(list));
        }

        public void Execute(List<string> argument)
        {
            var listArgs = converter.Convert(argument);
            var clientSpecification = specificatinsFactory.GetClientSpecification(listArgs);
            var formatter = taskFormatterFactory.GetFormatter(clientSpecification);
            var tasks = client.GetTasks(clientSpecification);

            if (tasks.Any())
            {
                PrintWithFormatter(tasks, formatter);
                return;
            }
            textWriter.WriteLine("Tasks not found");
            }
    }

    public class ListTests
    {
        private readonly IClient connection = Substitute.For<IClient>();
        private readonly ArgumentConverter<ListTaskArgs> converter = Substitute.For<ArgumentConverter<ListTaskArgs>>();
        private readonly ITaskFormatterFactory formatterFactory = Substitute.For<ITaskFormatterFactory>();
        private IClientSpecification data;
        private readonly StringBuilder sb = new StringBuilder();
        readonly StringWriter writer;
        private readonly ListCommand list;
        private readonly IClientSpecificatinsFactory specificatinsFactory =
            Substitute.For<IClientSpecificatinsFactory>();

        public ListTests()
        {
            writer = new StringWriter(sb);
            list = new ListCommand(converter, writer, formatterFactory, connection, specificatinsFactory);
        }

        [Fact]
        public void property_name_should_be_classname()
        {
            list.Name.Should().Be("list");
        }

        [Fact]
        public void should_get_all_commands()
        {
            data = new ListAllClientSpecification();
            var input = new List<string> { "153" };
            converter.Convert(input).Returns(new ListTaskArgs { Id = 153 });
            connection.GetTasks(data).ReturnsForAnyArgs(new List<ClientPackage>());

            list.Execute(input);
            connection.ReceivedWithAnyArgs().GetTasks(data);
        }
        [Fact]
        public void should_get_one_command_by_id()
        {
            data = new ListSingleClientSpecification();
            var input = new List<string>();
            connection.GetTasks(data).ReturnsForAnyArgs(new List<ClientPackage>());
            converter.Convert(input).Returns(new ListTaskArgs { Id = null });

            list.Execute(input);
            connection.ReceivedWithAnyArgs().GetTasks(data);
        }
        [Fact]
        public void should_get_one_command_by_date()
        {
            data = new ListTodayClientSpecification();
            var input = new List<string>();
            connection.GetTasks(data).ReturnsForAnyArgs(new List<ClientPackage>());
            converter.Convert(input).Returns(new ListTaskArgs { Id = 0, DueDate = DateTime.Now });

            list.Execute(input);
            connection.ReceivedWithAnyArgs().GetTasks(data);
        }

        [Fact]
        public void should_print_info_if_no_tasks_found()
        {
            data = new ListAllClientSpecification();
            var input = new List<string> { "153" };
            converter.Convert(input).Returns(new ListTaskArgs { Id = 153 });
            connection.GetTasks(data).ReturnsForAnyArgs(new List<ClientPackage>());

            list.Execute(input);
            sb.ToString().Should().BeEquivalentTo("Tasks not found\r\n");
        }

        [Fact]
        public void should_print_info_on_reauired_tasks()
        {

            var args = new ListTaskArgs { Id = 153 };
            var input = new List<string> { "153" };
            var listPackage = new List<ClientPackage> { new ClientPackage { DueDate = DateTime.Now, Id = 1, IsCompleted = true } };

            var formatter = Substitute.For<ITaskFormatter>();
            data = Substitute.For<IClientSpecification>();

            converter.Convert(input).Returns(args);
            specificatinsFactory.GetClientSpecification(args).Returns(data);
            formatterFactory.GetFormatter(data).Returns(formatter);
            formatter.ToFormatString(listPackage).Returns("hello world");

            connection.GetTasks(data).Returns(listPackage);

            list.Execute(input);
            sb.ToString().Should().BeEquivalentTo("hello world\r\n");
        }
    }
}