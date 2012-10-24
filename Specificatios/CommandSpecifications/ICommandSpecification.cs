using EntitiesLibrary;

namespace Specifications.CommandSpecifications
{
    public interface ICommandSpecification
    {
        bool IsSatisfied(ServiceTask task);
    }
}
