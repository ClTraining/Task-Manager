using EntitiesLibrary;

namespace CommandQueryLibrary.ServiceSpecifications
{
    public interface IServiceSpecification
    {
        bool IsSatisfied(ServiceTask task);
    }
}
