using EntitiesLibrary.CommandArguments;
using Specifications.ClientSpecifications;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public interface IClientSpecificatinsFactory
    {
        IClientSpecification GetClientSpecification(ListTaskArgs listArgs);
    }
}
