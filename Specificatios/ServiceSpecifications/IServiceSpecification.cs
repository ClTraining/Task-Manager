using EntitiesLibrary;
using Specifications.ClientSpecification;

namespace Specifications.ServiceSpecifications
{
    public interface IServiceSpecification
    {
        bool IsSatisfied(ServiceTask task);
    }
}
