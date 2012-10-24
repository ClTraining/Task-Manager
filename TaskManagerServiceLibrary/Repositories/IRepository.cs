using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Specifications.QuerySpecifications;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(AddTaskArgs args);
        List<ContractTask> GetTasks(IQuerySpecification spec);
        void Complete(CompleteTaskArgs args);
        void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateArgs args);
        void ClearTaskDueDate(ClearDateArgs args);
    }
}