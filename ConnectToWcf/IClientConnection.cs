using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
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
        void ClearTaskDueDate(ClearDateArgs args);
    }
}