using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Specifications.ClientSpecifications;

namespace ConnectToWcf
{
    public interface IClient
    {
        int AddTask(AddTaskArgs task);
        List<ClientPackage> GetTasks(IClientSpecification data);
        void Complete(CompleteTaskArgs args);
        void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateTaskArgs args);
        void ClearTaskDueDate(ClearDateTaskArgs args);
    }
}