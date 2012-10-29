using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Specifications.ServiceSpecifications;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(AddTaskArgs args);
        List<ServiceTask> GetTasks(IServiceSpecification spec);
        void UpdateChanges(ServiceTask task);
    }
}