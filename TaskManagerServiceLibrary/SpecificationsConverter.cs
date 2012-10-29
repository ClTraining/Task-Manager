using AutoMapper;
using CQRS.ClientSpecifications;
using CQRS.ServiceSpecifications;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary
{
    public class SpecificationsConverter : ISpecificationsConverter
    {
        public SpecificationsConverter()
        {
            Mapper.CreateMap<ListAllClientSpecification, ListAllServiceSpecification>();
            Mapper.CreateMap<ListByDateClientSpecification, ListByDateServiceSpecification>();
            Mapper.CreateMap<ListSingleClientSpecification, ListSingleServiceSpecification>();
            Mapper.CreateMap<IClientSpecification, IServiceSpecification>().ConvertUsing<SpecificationMapConverter<IClientSpecification, IServiceSpecification>>();
        }

        public IServiceSpecification GetQuerySpecification(IClientSpecification specification)
        {
            var querySpecification = Mapper.DynamicMap<IClientSpecification, IServiceSpecification>(specification);
            return querySpecification;
        }
    }

    public class SpecConvTests
    {
        [Fact]
        public void should_return_listsinglespecification()
        {
            var cSpec = new ListSingleClientSpecification();
            var converter = new SpecificationsConverter();
            
            var result = converter.GetQuerySpecification(cSpec);
            
            result.Should().BeOfType<ListSingleServiceSpecification>();
        }

        [Fact]
        public void should_return_listallspecification()
        {
            var cSpec = new ListAllClientSpecification();
            var converter = new SpecificationsConverter();

            var result = converter.GetQuerySpecification(cSpec);

            result.Should().BeOfType<ListAllServiceSpecification>();
        }
    }
}