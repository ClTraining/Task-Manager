using CommandQueryLibrary.ClientSpecifications;
using CommandQueryLibrary.ServiceSpecifications;

namespace TaskManagerServiceLibrary
{
    public interface ISpecificationsConverter
    {
        IServiceSpecification GetQuerySpecification(IClientSpecification specification);
    }
}
