using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace Specifications.QuerySpecifications
{
    public class ListSingleSpecification : IQuerySpecification
    {
        public int Id { get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return Id == task.Id;
        }

        public void Initialise(object data)
        {
            Id = (int) data;
        }
    }

    public class ListSingleSpecTests
    {
        [Fact]
        public void should_return_true_if_equal_tasks()
        {
            var task = new ServiceTask {Id = 2};
            var spec = new ListSingleSpecification{Id = 2};

            var result = spec.IsSatisfied(task);
            
            result.Should().BeTrue();
        }
    }
}