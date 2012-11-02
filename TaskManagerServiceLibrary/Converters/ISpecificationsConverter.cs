using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary.Converters
{
    public interface ISpecificationsConverter
    {
        IServiceSpecification GetQuerySpecification(IListCommandArguments args);
    }
}
