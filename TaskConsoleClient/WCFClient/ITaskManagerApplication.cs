using EntitiesLibrary;

namespace TaskConsoleClient.WCFClient
{
    public interface ITaskManagerApplication
    {
        ContractTask AddTask(ContractTask task);
    }
}
