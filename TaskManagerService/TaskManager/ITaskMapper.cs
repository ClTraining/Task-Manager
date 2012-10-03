using EntitiesLibrary;

namespace TaskManagerService.TaskManager
{
    public interface ITaskMapper
    {
        ServiceTask ConvertToService(ContractTask task);
        ContractTask ConvertToContract(ServiceTask task);
    }
}
