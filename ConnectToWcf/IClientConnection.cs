using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.Arguments.AddTask;
using EntitiesLibrary.Arguments.SetDate;
using TaskManagerServiceLibrary;

namespace ConnectToWcf
{
    public interface IClientConnection
    {
        int AddTask(AddTaskArgs task);
        List<ContractTask> GetTasks(object data);
        
        //void Complete(CompleteTaskArgs id);
        //void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateArgs args);
    }
}