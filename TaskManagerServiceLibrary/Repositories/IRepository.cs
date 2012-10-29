using System.Collections.Generic;
using CQRS.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(AddTaskArgs args);
        List<ServiceTask> GetTasks(IServiceSpecification spec);
        void UpdateChanges(ServiceTask task);
    }
}