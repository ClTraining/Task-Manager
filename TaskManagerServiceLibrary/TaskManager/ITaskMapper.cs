using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface ITaskMapper
    {
        ServiceTask ConvertToService(ContractTask task);
        ContractTask ConvertToContract(ServiceTask task);
    }
}
