using EntitiesLibrary.CommandArguments;
using Specifications.ServiceSpecifications;

namespace TaskManagerServiceLibrary
{
    public interface ISpecificationsConverter
    {
        IServiceSpecification GetQuerySpecification(IListCommandArguments specification);
    }
}
