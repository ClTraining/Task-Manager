using System.Collections.Generic;
using EntitiesLibrary;

namespace ConnectToWcf
{
    public interface IClientConnection
    {
        int AddTask(string task);
        ContractTask GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        void MarkCompleted(int id);
    }
}
