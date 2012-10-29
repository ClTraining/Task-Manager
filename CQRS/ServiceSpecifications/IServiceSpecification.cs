using EntitiesLibrary;

namespace CQRS.ServiceSpecifications
{
    public interface IServiceSpecification
    {
        bool IsSatisfied(ServiceTask task);
    }
}
