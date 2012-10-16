using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface IToDoList
    {
        int AddTask(string name);
        ContractTask GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        void Complete(int id);
        void RenameTask(RenameTaskArgs args);
    }
}