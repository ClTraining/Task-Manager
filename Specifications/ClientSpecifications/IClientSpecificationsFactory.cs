using EntitiesLibrary.CommandArguments;

namespace Specifications.ClientSpecifications
{
    public interface IClientSpecificationsFactory
    {
        IClientSpecification GetClientSpecification(ListTaskArgs listArgs);
    }
}
