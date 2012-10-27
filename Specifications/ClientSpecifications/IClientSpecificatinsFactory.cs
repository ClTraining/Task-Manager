using EntitiesLibrary.CommandArguments;

namespace Specifications.ClientSpecifications
{
    public interface IClientSpecificatinsFactory
    {
        IClientSpecification GetClientSpecification(ListTaskArgs listArgs);
    }
}
