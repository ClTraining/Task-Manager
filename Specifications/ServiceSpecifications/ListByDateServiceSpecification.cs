using System;
using EntitiesLibrary;
using FluentAssertions;
using Specifications.ClientSpecifications;
using Xunit;

namespace Specifications.ServiceSpecifications
{
    public class ListByDateServiceSpecification : IServiceSpecification, IClientSpecification
    {
        public DateTime Date { get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return Date == task.DueDate;
        }
    }

    public class ListTodaySpecificationTests
    {
        ListByDateServiceSpecification spec = new ListByDateServiceSpecification();

        [Fact]
        public void should_return_task_if_task_exists()
        {
            var spec = new ListByDateServiceSpecification {Date = DateTime.Today};
            var task = new ServiceTask{DueDate = DateTime.Today};

            var result = spec.IsSatisfied(task);

            result.Should().BeTrue();
        }
    }
}
