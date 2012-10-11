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
        public IEnumerable<int> CountRange { get; private set; }
        private const int PosId = 5;
        private const int PosName = 15;
        private const int PosCompleted = 13;
        private readonly string format;

        public TableFormatter()
        {
            CountRange = Enumerable.Range(2, 1000);
            format = "{0,-" + PosId + "} | {1," + PosName + "} | {2," + PosCompleted + "}";
        }

        public void Show(List<ContractTask> tasks)
        {
            PrintHeader();
            tasks.ForEach(x => Console.WriteLine(format, x.Id, (x.Name.Length>PosName) ? x.Name.Remove(PosName):x.Name, x.IsCompleted ? "+" : "-"));
            
        }

        private void PrintHeader()
        {
            Console.WriteLine(format, "Id", "Name", "Completed");
        }
    }

    public class TableFormatterTests
    {
        [Fact]
        public void should_print_header_for_empty_list()
        {
            var formatter = new TableFormatter();
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            formatter.Show(new List<ContractTask>());
            sb.ToString().Should().Be("Id    |            Name |     Completed\r\n");
        }

        [Fact]
        public void should_console_out_task_table()
        {
            var formatter = new TableFormatter();
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            formatter.Show(new List<ContractTask>(){new ContractTask(){Id = 1, Name = "abcd123456789000000000000"}});
            sb.ToString().Should().Be("Id    |            Name |     Completed\r\n1     | abcd12345678900 |             -\r\n");
        }
    }
}