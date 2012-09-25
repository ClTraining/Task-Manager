using EntitiesLibrary;

namespace TaskManagerService.WCFServer
{
    public interface ITaskManagerApplication
    {
        ContractTask AddTask(ContractTask task);
    }
}
