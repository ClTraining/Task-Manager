using System;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Specifications.ClientSpecifications;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public class ClientSpecificatinsFactory : IClientSpecificatinsFactory
    {
        public IClientSpecification GetClientSpecification(ListTaskArgs listArgs)
        {
            IClientSpecification data;

            if (listArgs.Date != default(DateTime) && listArgs.Id == 0)
                data = new ListByDateClientSpecification { Date = listArgs.Date };
            else if (listArgs.Date == default(DateTime) && listArgs.Id != null)
                data = new ListSingleClientSpecification { Id = listArgs.Id.Value };
            else
                data = new ListAllClientSpecification();
            return data;
        }
    }

    public class ClientSpecificatinsFactoryTests
    {
        private readonly ClientSpecificatinsFactory factory = new ClientSpecificatinsFactory();

        [Fact]
        public void If_contains_dateTime_and_id_zero_returns_ListByDateClientSpecification()
        {
            var listTaskArgs = new ListTaskArgs {Id = 0, Date = DateTime.Now};
            var clientSpecification = factory.GetClientSpecification(listTaskArgs);

            clientSpecification.Should().BeOfType<ListByDateClientSpecification>();
        }
        [Fact]
        public void If__no_dateTime_set_and_id_is_not_zero_returns_ListSingleClientSpecification()
        {
            var listTaskArgs = new ListTaskArgs {Id = 5, Date = default(DateTime)};
            var clientSpecification = factory.GetClientSpecification(listTaskArgs);

            clientSpecification.Should().BeOfType<ListSingleClientSpecification>();
        }
        [Fact]
        public void If__no_dateTime_set_and_id_is_not_zero_returns_ListAllClientSpecification()
        {
            var listTaskArgs = new ListTaskArgs {Id = null, Date = default(DateTime)};
            var clientSpecification = factory.GetClientSpecification(listTaskArgs);

            clientSpecification.Should().BeOfType<ListAllClientSpecification>();
        }
    }
}