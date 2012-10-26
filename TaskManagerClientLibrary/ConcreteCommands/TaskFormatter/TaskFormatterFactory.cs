using FluentAssertions;
using NSubstitute;
using Specifications.ClientSpecifications;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands.TaskFormatter
{
    public class TaskFormatterFactory : ITaskFormatterFactory
    {
        private readonly ListTaskFormatter listTaskFormatter;
        private readonly SingleTaskFormatter singleTaskFormatter;

        public TaskFormatterFactory(SingleTaskFormatter singleTaskFormatter, ListTaskFormatter listTaskFormatter)
        {
            this.listTaskFormatter = listTaskFormatter;
            this.singleTaskFormatter = singleTaskFormatter;
        }

        public ITaskFormatter GetFormatter(IClientSpecification specification)
        {
            if (specification is ListSingleClientSpecification)
                return singleTaskFormatter;
            return listTaskFormatter;
        }
    }

    public class TaskFormatterFactoryTests
    {
        private readonly ListTaskFormatter listFormatter = Substitute.For<ListTaskFormatter>();
        private readonly SingleTaskFormatter singleFormatter = Substitute.For<SingleTaskFormatter>();
        private readonly TaskFormatterFactory taskFormatterFactory;

        public TaskFormatterFactoryTests()
        {
            taskFormatterFactory = new TaskFormatterFactory(singleFormatter, listFormatter);
        }

        [Fact]
        public void should_return_list_formatter()
        {
            var result = taskFormatterFactory.GetFormatter(new ListAllClientSpecification());
            result.Should().BeSameAs(listFormatter);
        }

        [Fact]
        public void should_return_single_formatter()
        {
            var result = taskFormatterFactory.GetFormatter(new ListSingleClientSpecification());
            result.Should().BeSameAs(singleFormatter);
        }
    }
}