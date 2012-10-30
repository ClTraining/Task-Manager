using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace CommandQueryLibrary.ServiceSpecifications
{
    public class ListSingleServiceSpecification : IServiceSpecification
    {
        public int Id { private get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return Id == task.Id;
        }
    }

    public class ListSingleSpecificationTests
    {
        [Fact]
        public void should_return_true_if_equal_tasks()
        {
            var task = new ServiceTask {Id = 2};
            var spec = new ListSingleServiceSpecification {Id = 2};

            var result = spec.IsSatisfied(task);

            result.Should().BeTrue();
        }
    }
}