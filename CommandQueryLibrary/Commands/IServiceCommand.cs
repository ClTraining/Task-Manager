using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace CommandQueryLibrary.Commands
{
    public interface IServiceCommand
    {
        ServiceTask Update(ICommandArguments args, ServiceTask task);
    }
}