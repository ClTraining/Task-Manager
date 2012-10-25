using EntitiesLibrary;

namespace Specifications.QuerySpecifications
{
    public interface IQuerySpecification
    {
        bool IsSatisfied(ServiceTask task);
        //void Initialise(object data);
    }
}
