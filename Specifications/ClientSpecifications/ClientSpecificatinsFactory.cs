using System;
using System.Collections.Generic;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Xunit;
using System.Linq;

namespace Specifications.ClientSpecifications
{
    public class ClientSpecificatinsFactory : IClientSpecificatinsFactory
    {
        private readonly List<IClientSpecification> specifications;

        public ClientSpecificatinsFactory(List<IClientSpecification> specifications)
        {
            this.specifications = specifications;
        }

        public IClientSpecification GetClientSpecification(ListTaskArgs listArgs)
        {
            return specifications.First(s => s.IsSatisfied(listArgs));
        }
    }

    public class ClientSpecificatinsFactoryTests
    {
        private readonly ClientSpecificatinsFactory factory;

        public ClientSpecificatinsFactoryTests()
        {
            factory = new ClientSpecificatinsFactory(new List<IClientSpecification>
                                                         {
                                                             new ListSingleClientSpecification(),
                                                             new ListTodayClientSpecification(),
                                                             new ListAllClientSpecification()
                                                         });
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