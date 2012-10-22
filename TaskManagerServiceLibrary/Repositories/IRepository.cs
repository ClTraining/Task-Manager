using System.Collections.Generic;
using EntitiesLibrary;
using Specifications.ServiceSpecifications;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(AddTaskArgs name);
        List<ContractTask> GetTasks(IServiceSpecification spec);
        void Complete(CompleteTaskArgs id);
        void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateArgs args);
    }
}