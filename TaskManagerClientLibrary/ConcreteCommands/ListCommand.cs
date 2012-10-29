using System;
using System.Collections.Generic;
using System.IO;
using CQRS.ClientSpecifications;
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
        private readonly ArgumentConverter<ListTaskArgs> converter;
        private readonly TextWriter textWriter;
        private readonly ITaskFormatterFactory taskFormatterFactory;

        public ListCommand(ArgumentConverter<ListTaskArgs> converter, TextWriter textWriter,
                    ITaskFormatterFactory taskFormatterFactory, IClient client)
        {
            Description = "Displays list of all tasks or single task, specified by ID.";
            this.converter = converter;
            this.textWriter = textWriter;
            this.taskFormatterFactory = taskFormatterFactory;
            this.client = client;
        }

        private void PrintWithFormatter(List<ClientTask> list, ITaskFormatter formatter)
        {
            textWriter.WriteLine(formatter.ToFormatString(list));
        }

        public void Execute(List<string> argument)
        {
            var listArgs = converter.Convert(argument);
            var clientSpecification = GetClientSpecification(listArgs);
            var formatter = taskFormatterFactory.GetFormatter(clientSpecification);
            var tasks = client.GetTasks(clientSpecification);

            PrintWithFormatter(tasks, formatter);
        }

        private IClientSpecification GetClientSpecification(ListTaskArgs listArgs)
        {
            IClientSpecification data;

            if (listArgs.DueDate != default(DateTime) && listArgs.Id == 0)
                data = new ListByDateClientSpecification { Date = listArgs.DueDate };
            else if (listArgs.DueDate == default(DateTime) && listArgs.Id != null)
                data = new ListSingleClientSpecification { Id = listArgs.Id.Value };
            else
                data = new ListAllClientSpecification();
            return data;
        }
    }

    public class ListTests
    {
        private readonly IClient connection = Substitute.For<IClient>();
        private readonly ArgumentConverter<ListTaskArgs> converter = Substitute.For<ArgumentConverter<ListTaskArgs>>();
        private readonly ITaskFormatterFactory formatter = Substitute.For<ITaskFormatterFactory>();
        private IClientSpecification data;
        private readonly ListCommand list;

        public ListTests()
        {
            list = new ListCommand(converter, new StringWriter(), formatter, connection);
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
            connection.GetTasks(data).ReturnsForAnyArgs(new List<ClientTask>());

            list.Execute(input);
            connection.ReceivedWithAnyArgs().GetTasks(data);
        }
        [Fact]
        public void should_get_one_command_by_id()
        {
            data = new ListSingleClientSpecification();
            var input = new List<string>();
            connection.GetTasks(data).ReturnsForAnyArgs(new List<ClientTask>());
            converter.Convert(input).Returns(new ListTaskArgs { Id = null });

            list.Execute(input);
            connection.ReceivedWithAnyArgs().GetTasks(data);
        }
        [Fact]
        public void should_get_one_command_by_date()
        {
            data = new ListByDateClientSpecification();
            var input = new List<string>();
            connection.GetTasks(data).ReturnsForAnyArgs(new List<ClientTask>());
            converter.Convert(input).Returns(new ListTaskArgs { Id = 0, DueDate = DateTime.Now });

            list.Execute(input);
            connection.ReceivedWithAnyArgs().GetTasks(data);
        }
    }
}