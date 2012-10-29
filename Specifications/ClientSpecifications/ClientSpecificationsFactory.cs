using System;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace Specifications.ClientSpecifications
{
    public class ClientSpecificationsFactory : IClientSpecificationsFactory
    {
        public IClientSpecification GetClientSpecification(ListTaskArgs listArgs)
        {
            IClientSpecification data;

            if (listArgs.DueDate != default(DateTime) && listArgs.Id == 0)
                data = new ListTodayClientSpecification { Date = listArgs.DueDate };
            else if (listArgs.DueDate == default(DateTime) && listArgs.Id != null)
                data = new ListSingleClientSpecification { Id = listArgs.Id.Value };
            else
                data = new ListAllClientSpecification();
            return data;
        }
    }

    public class ClientSpecificatinsFactoryTests
    {
        private readonly ClientSpecificationsFactory factory;

        public ClientSpecificatinsFactoryTests()
        {
            factory = new ClientSpecificationsFactory();
        }

        [Fact]
        public void should_return_list_single_spec()
        {
            var args = new ListTaskArgs{Id = 1};
            var clientSpecification = factory.GetClientSpecification(args);
            clientSpecification.Should().BeOfType<ListSingleClientSpecification>();
        }

        [Fact]
        public void should_return_list_all_spec()
        {
            var args = new ListTaskArgs { Id = null };
            var clientSpecification = factory.GetClientSpecification(args);
            clientSpecification.Should().BeOfType<ListAllClientSpecification>();
        }

        [Fact]
        public void should_return_list_today_spec()
        {
            var args = new ListTaskArgs {DueDate = DateTime.Today};
            var clientSpecification = factory.GetClientSpecification(args);
            clientSpecification.Should().BeOfType<ListTodayClientSpecification>();
        }
    }
}