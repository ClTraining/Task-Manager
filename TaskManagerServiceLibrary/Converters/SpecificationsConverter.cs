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
            if (args is ListSingleTaskArgs)
                return new ListSingleServiceSpecification { Id = (args as ListSingleTaskArgs).Id };
            if (args is ListByDateTaskArgs)
                return new ListByDateServiceSpecification {Date = (args as ListByDateTaskArgs).Date};
            return new ListAllServiceSpecification();
        }
    }

    public class SpecConvTests
    {
        [Fact]
        public void should_return_listsinglespecification()
        {
            var args = new ListSingleTaskArgs();
            var converter = new SpecificationsConverter();
            
            var result = converter.GetQuerySpecification(args);
            
            result.Should().BeOfType<ListSingleServiceSpecification>();
        }

        [Fact]
        public void should_return_listallspecification()
        {
            var args = new ListAllTaskArgs();
            var converter = new SpecificationsConverter();

            var result = converter.GetQuerySpecification(args);

            result.Should().BeOfType<ListAllServiceSpecification>();
        }

        [Fact]
        public void should_return_listt_bydatespecification()
        {
            var args = new ListByDateTaskArgs();
            var converter = new SpecificationsConverter();

            var result = converter.GetQuerySpecification(args);

            result.Should().BeOfType<ListByDateServiceSpecification>();
        }
    }
}