using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace Specifications.QuerySpecifications
{
    public class ListAllSpecification : IQuerySpecification
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
            var spec = new ListAllSpecification();
            var result = spec.IsSatisfied(new ServiceTask());
            result.Should().BeTrue();
        }
    }
}
