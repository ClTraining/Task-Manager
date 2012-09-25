using EntitiesLibrary;

namespace TaskConsoleClient.Manager
{
    public interface ICommandManager
    {
        ContractTask AddTask(ContractTask task);
    }
}
