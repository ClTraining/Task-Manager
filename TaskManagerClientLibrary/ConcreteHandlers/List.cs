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
    public class List : Command<ListTaskArgs>
    {
        private readonly TaskFormatterFactory taskFormatterFactory;

        public List(IClientConnection client, ArgumentConverter<ListTaskArgs> converter, TextWriter textWriter,
                   TaskFormatterFactory taskFormatterFactory)
            : base(client, converter, textWriter)
        {
            Description = "Displays task list or single task by ID";
            this.taskFormatterFactory = taskFormatterFactory;
        }

        private void GetTasksAndPrint(Func<IClientConnection, List<ContractTask>> func, ITaskFormatter formatter)
        {
            var tasks = func(client);
            OutText(formatter.Show(tasks));
        }

        protected override void ExecuteWithGenericInput(ListTaskArgs input)
        {
            if (input.Date == default(DateTime) && input.Id == 0)
                GetTasksAndPrint(s => s.GetTasks(new ListAll()), taskFormatterFactory.GetListFormatter());
            if (input.Date != default(DateTime) && input.Id == 0)
                GetTasksAndPrint(s => s.GetTasks(new ListByDate(input.Date)), taskFormatterFactory.GetListFormatter());
            if (input.Date == default(DateTime) && input.Id != 0)
                GetTasksAndPrint(s => s.GetTasks(new ListSingle(input.Id)), taskFormatterFactory.GetListFormatter());
        }
    }

    public class ListTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ArgumentConverter<ListTaskArgs> converter = Substitute.For<ArgumentConverter<ListTaskArgs>>();
        private readonly SingleTaskFormatter formatter1 = Substitute.For<SingleTaskFormatter>();
        private readonly ListTaskFormatter formatter2 = Substitute.For<ListTaskFormatter>();
        private readonly List list;
        private readonly TaskFormatterFactory taskFormatterFactory;
    }
}