using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerService.TaskManager
{
    public interface IToDoList
    {
        int AddTask(string name);
        ContractTask GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        void MarkCompleted(int id);
    }
}