using AutoMapper;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace TaskManagerServiceLibrary
{
    public class SpecificationsConverter : ISpecificationsConverter
    {
        public IServiceSpecification GetQuerySpecification(IListCommandArguments args)
        {
            if (args is ListSingleTaskArgs)
                return new ListSingleServiceSpecification { Id = ((ListSingleTaskArgs)args).Id };
            else if (args is ListByDateTaskArgs)
                return new ListByDateServiceSpecification {Date = ((ListByDateTaskArgs) args).Date};
            else return new ListAllServiceSpecification();
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