using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands.TaskFormatter
{
    public class ListTaskFormatter : ITaskFormatter
    {
        private const int PosId = 5;
        private const int PosName = 15;
        private const int PosCompleted = 13;
        private const int PosDueDate = 20;
        private readonly string format;
        private readonly string datePattern = CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern;

        public ListTaskFormatter()
        {
            format = "{0,-" + PosId + "} | {1," + PosName + "} | {2," + PosCompleted + "} | {3," + PosDueDate + "}";
        }

        public virtual string ToFormatString(List<ClientTask> tasks)
        {
            var taskString = new StringBuilder();
            taskString.AppendLine(PrintHeader());

            tasks.ForEach(
                x =>
                taskString.AppendLine(String.Format(format, x.Id,
                                                    (x.Name.Length > PosName) ? x.Name.Remove(PosName) : x.Name,
                                                    x.IsCompleted ? "+" : "-", x.DueDate == default(DateTime) ? "not set" : x.DueDate.ToString(datePattern))));

            return taskString.ToString();
        }

        private string PrintHeader()
        {
            return string.Format(format, "Id", "Name", "Completed", "Due date");
        }
    }

    public class ListTaskFormatterTests
    {
        [Fact]
        public void should_print_header_for_empty_list()
        {
            var formatter = new ListTaskFormatter();
            var sb = new StringBuilder();
            sb.Append(formatter.ToFormatString(new List<ClientTask>()));
            sb.ToString().Should().Be("Id    |            Name |     Completed |             Due date\r\n");
        }

        [Fact]
        public void should_console_out_task_table()
        {
            var formatter = new ListTaskFormatter();
            var sb = new StringBuilder();
            sb.Append(
                formatter.ToFormatString(new List<ClientTask> { new ClientTask { Id = 1, Name = "abcd123456789000000000000" } }));
            sb.ToString().Should().Be(
                "Id    |            Name |     Completed |             Due date\r\n1     | abcd12345678900 |             - |              not set\r\n");
        }
    }
}