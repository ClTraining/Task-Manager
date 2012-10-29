using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace CQRS.Commands
{
    public interface IServiceCommand
    {
        ServiceTask Update(ICommandArguments args, ServiceTask task);
    }
}