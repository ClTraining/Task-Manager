using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Specifications.ServiceSpecifications;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(AddTaskArgs args);
        List<ClientPackage> GetTasks(IServiceSpecification spec);
        void Complete(CompleteTaskArgs args);
        void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateTaskArgs args);
        void ClearTaskDueDate(ClearDateTaskArgs args);
    }
}