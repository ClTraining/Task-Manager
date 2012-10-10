using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface ITaskMapper
    {
        ContractTask ConvertToContract(ServiceTask task);
    }
}
