using CQRS.ClientSpecifications;
using CQRS.ServiceSpecifications;

namespace TaskManagerServiceLibrary
{
    public interface ISpecificationsConverter
    {
        IServiceSpecification GetQuerySpecification(IClientSpecification specification);
    }
}
