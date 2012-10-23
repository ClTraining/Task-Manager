using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.Arguments.AddTask;
using EntitiesLibrary.Arguments.CompleteTask;
using EntitiesLibrary.Arguments.RenameTask;
using EntitiesLibrary.Arguments.SetDate;
using Specifications.ClientSpecification;

namespace ConnectToWcf
{
    public interface IClientConnection
    {
        int AddTask(AddTaskArgs task);
        List<ContractTask> GetTasks(IClientSpecification data);
        void Complete(CompleteTaskArgs args);
        void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateArgs args);
    }
}