using System;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace Specifications.ServiceSpecifications
{
    public class ListTodayServiceSpecification : IServiceSpecification
    {
        public DateTime Date { get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return Date == task.DueDate;
        }
    }

    public class ListTodaySpecificationTests
    {
        ListTodayServiceSpecification spec = new ListTodayServiceSpecification();

        [Fact]
        public void should_return_task_if_task_exists()
        {
            var spec = new ListTodayServiceSpecification {Date = DateTime.Today};
            var task = new ServiceTask{DueDate = DateTime.Today};

            var result = spec.IsSatisfied(task);

            result.Should().BeTrue();
        }
    }
}
