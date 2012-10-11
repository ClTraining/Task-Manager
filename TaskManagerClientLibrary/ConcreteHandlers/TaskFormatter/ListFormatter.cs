using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter
{
    public class ListFormatter : ITaskFormatter
    {
        public IEnumerable<int> CountRange { get; private set; }

        public ListFormatter()
        {
            CountRange = Enumerable.Range(1, 1);
        }

        public void Show(List<ContractTask> tasks)
        {
            tasks.ForEach(x=>Console.Write("\nID:\t\t{0}\n" +
                                           "Name:\t\t{1}\n" +
                                           "Completed:\t{2}\n\n", x.Id, x.Name, x.IsCompleted ? "+" : "-"));
        }
    }

    public class ListFormatterTests
    {
        [Fact]
        public void should_correctly_out_one_task()
        {
            var sb = new StringBuilder();
            var tasks = new List<ContractTask> {new ContractTask {Id = 1, Name = "task1", IsCompleted = false}};
            var formatter = new ListFormatter();
            Console.SetOut(new StringWriter(sb));
            formatter.Show(tasks);
            sb.ToString().Should().Be("\nID:\t\t1\n" +
                                               "Name:\t\ttask1\n" +
                                               "Completed:\t-\n\n");
        }
    }
}