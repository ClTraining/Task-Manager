using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace CommandQueryLibrary.ServiceSpecifications
{
    public class ListAllServiceSpecification : IServiceSpecification
    {
        public bool IsSatisfied(ServiceTask task)
        {
            return true;
        }
    }

    public class ListAllSpecificationTests
    {
        [Fact]
        public void should_always_return_true_if_task_exists()
        {
            var spec = new ListAllServiceSpecification();
            var result = spec.IsSatisfied(new ServiceTask());
            result.Should().BeTrue();
        }
    }
}
