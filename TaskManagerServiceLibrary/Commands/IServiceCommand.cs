using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Commands
{
    public interface IServiceCommand
    {
        ServiceTask ExecuteCommand(ServiceTask task);
    }
}