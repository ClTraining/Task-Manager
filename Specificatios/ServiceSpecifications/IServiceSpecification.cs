using EntitiesLibrary;

namespace Specifications.ServiceSpecifications
{
    public interface IServiceSpecification
    {
        object Data { get; set; }
        bool IsSatisfied(ServiceTask task);
    }
}
