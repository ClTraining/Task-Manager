using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskConsoleClient.Manager
{
    public interface ICommandManager
    {
        int AddTask(string task);
        ContractTask GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        ContractTask Edit(ContractTask task);
    }
}
