using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace TaskManagerServiceLibrary.Converters
{
    public class SpecificationsConverter : ISpecificationsConverter
    {
        public IServiceSpecification GetQuerySpecification(IListCommandArguments args)
        {
            var listSingleTaskArgs = args as ListSingleTaskArgs;
            if (listSingleTaskArgs != null)
                return new ListSingleServiceSpecification { Id = listSingleTaskArgs.Id };
            var listByDateTaskArgs = args as ListByDateTaskArgs;
            if (listByDateTaskArgs != null)
                return new ListByDateServiceSpecification {Date = listByDateTaskArgs.Date};
            return new ListAllServiceSpecification();
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