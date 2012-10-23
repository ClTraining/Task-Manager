using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary;
using EntitiesLibrary.Arguments.ListTask;
using NSubstitute;
using Specifications.ClientSpecification;
using TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter;
using TaskManagerServiceLibrary;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class List : Command<ListArgs>
    {
        private readonly TaskFormatterFactory taskFormatterFactory;

        public List(IClientConnection client, ArgumentConverter<ListArgs> converter, TextWriter textWriter,
                    TaskFormatterFactory taskFormatterFactory)
            : base(client, converter, textWriter)
        {
            Description = "Displays list of all tasks or single task, specified by ID.";
            this.taskFormatterFactory = taskFormatterFactory;
        }

        private void GetTasksAndPrint(Func<IClientConnection, List<ContractTask>> func, ITaskFormatter formatter)
        {
            List<ContractTask> tasks = func(client);
            OutText(formatter.Show(tasks));
        }

        protected override void ExecuteWithGenericInput(ListArgs input)
        {
            object data;

            if (input.Date != default(DateTime) && input.Id == 0)
                data = new ListByDate{Data = input.Date};

            else if (input.Date == default(DateTime) && input.Id != null)
                data = new ListSingle { Data = input.Id.Value };

            else data = new ListAll { Data = null };

            GetTasksAndPrint(s => s.GetTasks(data), input.Id == null
                                                        ? taskFormatterFactory.GetListFormatter()
                                                        : taskFormatterFactory.GetSingleFormatter()
                );
        }
    }

    public class ListTests
    {
        private readonly IClientConnection connection = Substitute.For<IClientConnection>();
        private readonly ArgumentConverter<ListArgs> converter = Substitute.For<ArgumentConverter<ListArgs>>();
        private readonly IClientSpecification specification = Substitute.For<IClientSpecification>();
        private readonly TaskFormatterFactory formatter = Substitute.For<TaskFormatterFactory>();
        private object data;
        private readonly List list;

        public ListTests()
        {
            list = new List(connection, converter, new StringWriter(), formatter);
        }

        [Fact]
        public void should_get_all_commnads()
        {
            data = new ListAll();
            var input = new List<string> { "153" };
            converter.Convert(input).Returns((object)new ListArgs { Id = 153 });

            list.Execute(input);
            connection.ReceivedWithAnyArgs().GetTasks(data);
        }
        [Fact]
        public void should_get_one_command_by_id()
        {
            data = new ListSingle();
            var input = new List<string>();
            converter.Convert(input).Returns(new ListArgs { Id = null });

            list.Execute(input);
            connection.ReceivedWithAnyArgs().GetTasks(data);
        }
        [Fact]
        public void should_get_one_command_by_date()
        {
            data = new ListByDate();
            var input = new List<string>();
            converter.Convert(input).Returns(new ListArgs { Id = 0, Date = DateTime.Now });

            list.Execute(input);
            connection.ReceivedWithAnyArgs().GetTasks(data);
        }
    }
}