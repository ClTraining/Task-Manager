using EntitiesLibrary;

namespace Specifications.ServiceSpecifications
{
    public interface IServiceSpecification
    {
        bool IsSatisfied(ServiceTask task);
    }
}
