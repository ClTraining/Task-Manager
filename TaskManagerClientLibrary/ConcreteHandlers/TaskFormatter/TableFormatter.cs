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
    public class TableFormatter : ITaskFormatter
    {
        public TableFormatter()
        {
            CountRange = Enumerable.Range(2, 1000);
        }

        public void Show(List<ContractTask> tasks)
        {
            Console.Write("table");
        }

        public IEnumerable<int> CountRange { get; set; }
    }

    public class TableFormatterTests
    {
        [Fact]
        public void should_console_out_task_table()
        {
            var formatter = new TableFormatter();
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            formatter.Show(new List<ContractTask>());
            sb.ToString().Should().Be("table");
        }
    }
}