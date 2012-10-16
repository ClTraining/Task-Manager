using System.Collections.Generic;
using EntitiesLibrary;

namespace ConnectToWcf
{
    public interface IClientConnection
    {
        int AddTask(string task);
        List<ContractTask> GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        void Complete(int id);
        void RenameTask(RenameTaskArgs args);
    }
}