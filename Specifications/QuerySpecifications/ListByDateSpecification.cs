using System;
using EntitiesLibrary;
using FluentAssertions;
using Specifications.ClientSpecification;
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

        //public void Initialise(object data)
        //{
        //    Date = ((ListByDate)data).Date;
        //}
    }

    public class ListByDateSpecificationTests
    {
        ListByDateSpecification spec = new ListByDateSpecification();

        [Fact]
        public void should_return_task_if_task_exists()
        {
            var spec = new ListByDateSpecification {Date = DateTime.Today};
            var task = new ServiceTask{DueDate = DateTime.Today};

            var result = spec.IsSatisfied(task);

            result.Should().BeTrue();
        }

        [Fact]
        public void test1()
        {
        }

        [Fact]
        public void should_initialise_specification()
        {
            //var dateTime = DateTime.Today;
            //var spec = new ListByDate {Date = dateTime};
            //spec.Initialise(dateTime);
            //spec.Date.Should().Be(dateTime);
        }
    }
}
