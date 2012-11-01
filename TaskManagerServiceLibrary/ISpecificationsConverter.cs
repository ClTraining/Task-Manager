using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary
{
    public interface ISpecificationsConverter
    {
        IServiceSpecification GetQuerySpecification(IListCommandArguments specification);
    }
}
