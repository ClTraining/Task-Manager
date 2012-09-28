using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerHost.TaskManager
{
    public interface IToDoList
    {
        int AddTask(string name);
        ContractTask GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        bool MarkCompleted(int id);
    }
}