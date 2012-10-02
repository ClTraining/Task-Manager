using EntitiesLibrary;

namespace TaskManagerHost.TaskManager
{
    public interface ITaskMapper
    {
        ServiceTask ConvertToService(ContractTask task);
        ContractTask ConvertToContract(ServiceTask task);
    }
}
