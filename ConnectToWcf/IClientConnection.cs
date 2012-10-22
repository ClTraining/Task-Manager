using System.Collections.Generic;
using EntitiesLibrary;
using Specifications.ClientSpecification;

namespace ConnectToWcf
{
    public interface IClientConnection
    {
        int AddTask(AddTaskArgs task);
        List<ContractTask> GetTasks(IClientSpecification id);
        
        //void MarkTaskAsCompleted(CompleteTaskArgs id);
        //void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateArgs args);
    }
}