using Specifications.ClientSpecifications;
using Specifications.ServiceSpecifications;

namespace TaskManagerServiceLibrary
{
    public interface ISpecificationsConverter
    {
        IServiceSpecification GetQuerySpecification(IClientSpecification specification);
    }
}
