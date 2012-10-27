using EntitiesLibrary.CommandArguments;

namespace Specifications.ClientSpecifications
{
    public interface IClientSpecification
    {
        bool IsSatisfied(ListTaskArgs listArgs);
    }
}
