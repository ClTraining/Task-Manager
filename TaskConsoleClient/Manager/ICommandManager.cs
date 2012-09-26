using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskConsoleClient.Manager
{
    public interface ICommandManager
    {
        ContractTask AddTask(ContractTask task);
        void ViewTaskById(int id);
        void ViewAllTasks();
        ContractTask Edit(ContractTask task);
    }
}
