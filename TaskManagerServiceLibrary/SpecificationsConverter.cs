using AutoMapper;
using Specifications.ClientSpecification;
using Specifications.QuerySpecifications;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary
{
    public class SpecificationsConverter : ISpecificationsConverter
    {
        public SpecificationsConverter()
        {
            Mapper.CreateMap<ListAll, ListAllSpecification>();
            Mapper.CreateMap<ListByDate, ListByDateSpecification>();
            Mapper.CreateMap<ListSingle, ListSingleSpecification>();
            Mapper.CreateMap<IClientSpecification, IQuerySpecification>().ConvertUsing(new SpecificationMapConverter<IClientSpecification, IQuerySpecification>());
        }

        public IQuerySpecification GetQuerySpecification(IClientSpecification specification)
        {
            var querySpecification = Mapper.DynamicMap<IClientSpecification, IQuerySpecification>(specification);
            return querySpecification;
        }
    }

    public class SpecConvTests
    {
        [Fact]
        public void should_return_listsinglespecification()
        {
            var cSpec = new ListSingle();
            new ListSingleSpecification();
            var converter = new SpecificationsConverter();
            
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