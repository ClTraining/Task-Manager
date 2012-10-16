using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter
{
    public class TaskFormatterFactory
    {
        private readonly SingleTaskFormatter singleTaskFormatter;
        private readonly ListTaskFormatter listTaskFormatter;

        public TaskFormatterFactory(SingleTaskFormatter singleTaskFormatter, ListTaskFormatter listTaskFormatter)
        {
            this.listTaskFormatter = listTaskFormatter;
            this.singleTaskFormatter = singleTaskFormatter;
        }

        public virtual ITaskFormatter GetListFormatter()
        {
            return listTaskFormatter;
        }

        public virtual ITaskFormatter GetSingleFormatter()
        {
            return singleTaskFormatter;
        }
    }

    public class TaskFormatterFactoryTests
    {
        private readonly TaskFormatterFactory taskFormatterFactory;
        private readonly ListTaskFormatter listFormatter = Substitute.For<ListTaskFormatter>();
        private readonly SingleTaskFormatter singleFormatter = Substitute.For<SingleTaskFormatter>();

        public TaskFormatterFactoryTests()
        {
            taskFormatterFactory = new TaskFormatterFactory(singleFormatter, listFormatter);
        }

        [Fact]
        public void should_return_list_formatter()
        {
            var result = taskFormatterFactory.GetListFormatter();
            result.Should().BeSameAs(listFormatter);
        }

        [Fact]
        public void should_return_single_formatter()
        {
            var result = taskFormatterFactory.GetSingleFormatter();
            result.Should().BeSameAs(singleFormatter);
        }
    }
}
