using System;
using System.Collections.Generic;
using System.Text;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter
{
    public class SingleTaskFormatter : ITaskFormatter
    {
        private const string Format = "\nID:\t\t{0}\n" + "Name:\t\t{1}\n" + "Completed:\t{2}\n" + "Due date:\t{3}\n\n";

        #region ITaskFormatter Members

        public virtual string Show(List<ContractTask> tasks)
        {
            var taskString = new StringBuilder();

            DateTime minValue = DateTime.MinValue;
            tasks.ForEach(
                x =>
                taskString.Append(String.Format(Format, x.Id, x.Name, x.IsCompleted ? "+" : "-",
                                                x.DueDate == minValue ? " not set" : x.DueDate.ToString())));

            return taskString.ToString();
        }

        #endregion
    }

    public class SingleTaskFormatterTests
    {
        [Fact]
        public void should_correctly_out_one_task()
        {
            var sb = new StringBuilder();
            var tasks = new List<ContractTask> {new ContractTask {Id = 1, Name = "task1", IsCompleted = false}};
            var formatter = new SingleTaskFormatter();
            sb.Append(formatter.Show(tasks));
            sb.ToString().Should().Be("\nID:		1\nName:		task1\nCompleted:	-\nDue date:	 not set\n\n");
        }
    }
}