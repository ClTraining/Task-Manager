using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface IToDoList
    {
        int AddTask(AddTaskArgs name);
        ContractTask GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        void MarkTaskAsCompleted(CompleteTaskArgs id);
        void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateArgs args);
    }
}