using AutoMapper;
using EntitiesLibrary.CommandArguments;
using CommandQueryLibrary.ServiceSpecifications;
using FluentAssertions;
using Xunit;

namespace TaskManagerServiceLibrary
{
    public class SpecificationsConverter : ISpecificationsConverter
    {
        public SpecificationsConverter()
        {
            Mapper.CreateMap<ListAllTaskArgs, ListAllServiceSpecification>();
            Mapper.CreateMap<ListByDateTaskArgs, ListByDateServiceSpecification>();
            Mapper.CreateMap<ListSingleTaskArgs, ListSingleServiceSpecification>();
            Mapper.CreateMap<IListCommandArguments, IServiceSpecification>().ConvertUsing<MapConverter<IListCommandArguments, IServiceSpecification>>();
        }

        public IServiceSpecification GetQuerySpecification(IListCommandArguments specification)
        {
            var querySpecification = Mapper.DynamicMap<IListCommandArguments, IServiceSpecification>(specification);
            return querySpecification;
        }
    }

    public class SpecConvTests
    {
        [Fact]
        public void should_return_listsinglespecification()
        {
            var cSpec = new ListSingleTaskArgs();
            var converter = new SpecificationsConverter();
            
            var result = converter.GetQuerySpecification(cSpec);
            
            result.Should().BeOfType<ListSingleServiceSpecification>();
        }

        [Fact]
        public void should_return_listallspecification()
        {
            var cSpec = new ListAllTaskArgs();
            var converter = new SpecificationsConverter();

            var result = converter.GetQuerySpecification(cSpec);

            result.Should().BeOfType<ListAllServiceSpecification>();
        }
    }
}