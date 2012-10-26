using FluentAssertions;
using Specifications.ClientSpecifications;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands.TaskFormatter
{
    public class TaskFormatterFactory : ITaskFormatterFactory
    {
        public ITaskFormatter GetFormatter(IClientSpecification specification)
        {
            if (specification is ListSingleClientSpecification)
                return new SingleTaskFormatter();
            return new ListTaskFormatter();
        }
    }

    public class TaskFormatterFactoryTests
    {
        private readonly TaskFormatterFactory taskFormatterFactory;

        public TaskFormatterFactoryTests()
        {
            taskFormatterFactory = new TaskFormatterFactory();
        }

        [Fact]
        public void should_return_list_formatter()
        {
            var result = taskFormatterFactory.GetFormatter(new ListAllClientSpecification());
            result.Should().BeOfType<ListTaskFormatter>();
        }

        [Fact]
        public void should_return_single_formatter()
        {
            var result = taskFormatterFactory.GetFormatter(new ListSingleClientSpecification());
            result.Should().BeOfType<SingleTaskFormatter>();
        }
    }
}