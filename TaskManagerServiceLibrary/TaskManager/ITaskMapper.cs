using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface ITaskMapper
    {
        ClientPackage ConvertToContract(ServiceTask task);
        ServiceTask Convert(ICommandArguments fromArgs, ServiceTask toTask);
    }
}