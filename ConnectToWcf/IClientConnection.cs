using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace ConnectToWcf
{
    public interface IClientConnection
    {
        int AddTask(AddTaskArgs task);
        List<ContractTask> GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        void MarkTaskAsCompleted(CompleteTaskArgs id);
        void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateArgs args);
    }
}