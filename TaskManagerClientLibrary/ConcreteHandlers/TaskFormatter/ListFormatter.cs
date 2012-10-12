using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter
{
    public class ListFormatter : ITaskFormatter
    {
        private const string format = "\nID:\t\t{0}\n" + "Name:\t\t{1}\n" + "Completed:\t{2}\n\n";

        public string Show(List<ContractTask> tasks)
        {
            var taskString = new StringBuilder();

            tasks.ForEach(x => taskString.Append(String.Format(format, x.Id, x.Name, x.IsCompleted ? "+" : "-")));

            return taskString.ToString();
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
            sb.Append(formatter.Show(tasks));
            sb.ToString().Should().Be("\nID:\t\t1\n" +
                                               "Name:\t\ttask1\n" +
                                               "Completed:\t-\n\n");
        }
    }
}