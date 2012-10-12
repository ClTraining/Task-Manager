using System;
using System.Collections.Generic;
using System.Text;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter
{
    public class TableFormatter : ITaskFormatter
    {
        private const int PosId = 5;
        private const int PosName = 15;
        private const int PosCompleted = 13;
        private readonly string format;

        public TableFormatter()
        {
            format = "{0,-" + PosId + "} | {1," + PosName + "} | {2," + PosCompleted + "}";
        }

        public string Show(List<ContractTask> tasks)
        {
            var taskString = new StringBuilder();
            taskString.AppendLine(PrintHeader());

            tasks.ForEach(x => taskString.AppendLine(String.Format(format, x.Id, (x.Name.Length > PosName) ? x.Name.Remove(PosName) : x.Name, x.IsCompleted ? "+" : "-")));

            return taskString.ToString();
        }

        private string PrintHeader()
        {
            return string.Format(format, "Id", "Name", "Completed");
        }
    }

    public class TableFormatterTests
    {
        [Fact]
        public void should_print_header_for_empty_list()
        {
            var formatter = new TableFormatter();
            var sb = new StringBuilder();
            sb.Append(formatter.Show(new List<ContractTask>()));
            sb.ToString().Should().Be("Id    |            Name |     Completed\r\n");
        }

        [Fact]
        public void should_console_out_task_table()
        {
            var formatter = new TableFormatter();
            var sb = new StringBuilder();
            sb.Append(formatter.Show(new List<ContractTask>() { new ContractTask() { Id = 1, Name = "abcd123456789000000000000" } }));
            sb.ToString().Should().Be("Id    |            Name |     Completed\r\n1     | abcd12345678900 |             -\r\n");
        }
    }
}