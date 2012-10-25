using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface ITaskMapper
    {
        ClientPackage ConvertToContract(ServiceTask task);
    }
}