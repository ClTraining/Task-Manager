using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace Specifications.ClientSpecifications
{
    public class ClientSpecificationsFactory : IClientSpecificationsFactory
    {

        private readonly Dictionary<Func<ListTaskArgs, bool>, IClientSpecification> specificationDictionary;

        public ClientSpecificationsFactory()
        {
            specificationDictionary = new Dictionary<Func<ListTaskArgs, bool>, IClientSpecification>
                                          {
                                              {x => x.DueDate != default(DateTime), new ListByDateClientSpecification()},
                                              {x => x.Id != null, new ListSingleClientSpecification()},
                                              {x => x.DueDate == default(DateTime) && x.Id == null, new ListAllClientSpecification()}
                                          };
        }

        public IClientSpecification GetClientSpecification(ListTaskArgs listArgs)
        {
            IClientSpecification outSpec;
            var functions = specificationDictionary.Keys;
            var key = functions.First(x => x.Invoke(listArgs));
            specificationDictionary.TryGetValue(key, out outSpec);
            return outSpec;
        }
    }

    public class ClientSpecificationsFactoryTests
    {
        private readonly ClientSpecificationsFactory factory = new ClientSpecificationsFactory();
        private ListTaskArgs args;

        [Fact]
        public void when_argument_has_date_should_return_ListByDateClientSpecification()
        {
            args = new ListTaskArgs { DueDate = DateTime.Now };

            var clientSpecification = factory.GetClientSpecification(args);

            clientSpecification.Should().BeOfType<ListByDateClientSpecification>();
        }

        [Fact]
        public void when_argument_has_ID_should_return_ListSingleClientSpecification()
        {
            args = new ListTaskArgs { Id = 5 };

            var clientSpecification = factory.GetClientSpecification(args);

            clientSpecification.Should().BeOfType<ListSingleClientSpecification>();
        }

        [Fact]
        public void when_argument_has_default_date_and_ID_should_return_ListAllClientSpecification()
        {
            args = new ListTaskArgs();
            var clientSpecification = factory.GetClientSpecification(args);

            clientSpecification.Should().BeOfType<ListAllClientSpecification>();
        }
    }
}