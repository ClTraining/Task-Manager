using System;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace Specifications.QuerySpecifications
{
    public class ListByDateSpecification : IQuerySpecification
    {
        public DateTime Date { get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return Date == task.DueDate;
        }

        public void Initialise(object data)
        {
            Date = (DateTime)data;
        }
    }

    public class ListByDateSpecTests
    {
        [Fact]
        public void should_return_task_if_task_exists()
        {
            var spec = new ListByDateSpecification {Date = DateTime.Today};
            var task = new ServiceTask{DueDate = DateTime.Today};

            var result = spec.IsSatisfied(task);

            result.Should().BeTrue();
        }

        [Fact]
        public void should_initialise_specification()
        {
            var spec = new ListByDateSpecification();
            var dateTime = DateTime.Today;
            spec.Initialise(dateTime);
            spec.Date.Should().Be(dateTime);
        }
    }
}
