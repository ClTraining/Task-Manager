using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary;
using NSubstitute;
using Specifications.ClientSpecification;
using TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter;
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
            if (input.Date == default(DateTime) && input.Id == null)
                GetTasksAndPrint(s => s.GetTasks(new ListAll()), taskFormatterFactory.GetListFormatter());
            if (input.Date != default(DateTime) && input.Id == null)
                GetTasksAndPrint(s => s.GetTasks(new ListByDate(input.Date)), taskFormatterFactory.GetListFormatter());
            if (input.Date == default(DateTime) && input.Id != null)
                GetTasksAndPrint(s => s.GetTasks(new ListSingle(input.Id.Value)), taskFormatterFactory.GetListFormatter());
        }
    }

    public class ListTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ArgumentConverter<ListArgs> converter = Substitute.For<ArgumentConverter<ListArgs>>();
        private readonly SingleTaskFormatter formatter1 = Substitute.For<SingleTaskFormatter>();
        private readonly ListTaskFormatter formatter2 = Substitute.For<ListTaskFormatter>();
        private readonly List list;
        private readonly TaskFormatterFactory taskFormatterFactory;

        public ListTests()
        {
            taskFormatterFactory = Substitute.For<TaskFormatterFactory>(formatter1, formatter2);
            list = new List(client, converter, new StringWriter(), taskFormatterFactory);
            taskFormatterFactory.GetSingleFormatter().Returns(formatter1);
            taskFormatterFactory.GetListFormatter().Returns(formatter2);
        }
    }

}