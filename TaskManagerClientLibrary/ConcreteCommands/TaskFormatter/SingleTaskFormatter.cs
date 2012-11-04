using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands.TaskFormatter
{
    public class SingleTaskFormatter : ITaskFormatter
    {
        private const string Format = "\nID:\t\t{0}\n" + "Name:\t\t{1}\n" + "Completed:\t{2}\n" + "Due date:\t{3}\n\n";
        private const string TaskNotFound = "Specified task not found.";
        private readonly string datePattern = CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern;

        public virtual string ToFormatString(List<ClientTask> tasks)
        {
            var taskString = new StringBuilder();
            if (tasks.Count == 0)
            {
                taskString.Append(TaskNotFound);
            }
            else
            {
                tasks.ForEach(
                    x =>
                    taskString.Append(String.Format(Format, x.Id, x.Name, x.IsCompleted ? "+" : "-",
                                                    x.DueDate == default(DateTime)
                                                        ? "not set"
                                                        : x.DueDate.ToString(datePattern))));
            }
            return taskString.ToString();
        }
    }

    public class SingleTaskFormatterTests
    {
        SingleTaskFormatter formatter = new SingleTaskFormatter();
        
        [Fact]
        public void should_correctly_out_one_task()
        {
            var sb = new StringBuilder();
            var tasks = new List<ClientTask> { new ClientTask { Id = 1, Name = "task1", IsCompleted = false } };
            
            sb.Append(formatter.ToFormatString(tasks));
            sb.ToString().Should().Be("\nID:		1\nName:		task1\nCompleted:	-\nDue date:	not set\n\n");
        }

        [Fact]
        public void should_inform_that_tasks_not_found_if_arg_count_is_0()
        {
            var message = formatter.ToFormatString(new List<ClientTask>());

            message.Should().Be("Specified task not found.");
        }
    }
}