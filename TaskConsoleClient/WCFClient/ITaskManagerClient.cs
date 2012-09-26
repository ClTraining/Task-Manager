using EntitiesLibrary;

namespace TaskConsoleClient.WCFClient
{
    public interface ITaskManagerClient
    {
        ContractTask AddTask(ITask task);
    }
}
