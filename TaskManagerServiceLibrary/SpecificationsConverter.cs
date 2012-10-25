using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Specifications.ClientSpecification;
using Specifications.QuerySpecifications;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary
{
    public class SpecificationsConverter : ISpecificationsConverter
    {
        private readonly IEnumerable<IQuerySpecification> serviceSpecifications;

        public SpecificationsConverter(IEnumerable<IQuerySpecification> serviceSpecifications)
        {
            this.serviceSpecifications = serviceSpecifications;
        }

        public IQuerySpecification GetQuerySpecification(IClientSpecification specification)
        {
            var querySpecification = serviceSpecifications.First(x => x.GetType().Name.Contains(specification.GetType().Name));
            querySpecification.Initialise(specification.Data);
            return querySpecification;
        }
    }

    public class SpecConvTests
    {
        [Fact]
        public void should_return_listsinglespecification()
        {
            var cSpec = new ListSingle{Data = 1};
            var qSpec = new ListSingleSpecification();
            var converter = new SpecificationsConverter(new List<IQuerySpecification> {qSpec});
            
            var result = converter.GetQuerySpecification(cSpec);
            
            result.Should().BeOfType<ListSingleSpecification>();
        }

        [Fact]
        public void should_return_listallspecification()
        {
            var cSpec = new ListAll();
            var qSpec = new ListAllSpecification();
            var converter = new SpecificationsConverter(new List<IQuerySpecification> {qSpec});

            var result = converter.GetQuerySpecification(cSpec);
            result.Should().BeOfType<ListAllSpecification>();
        }
    }
}