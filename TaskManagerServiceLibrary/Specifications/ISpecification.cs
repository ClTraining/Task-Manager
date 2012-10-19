using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Specifications
{
    public interface ISpecification
    {
        bool IsSatisfied(ServiceTask task);
    }
}
