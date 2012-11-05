using System.Collections.Generic;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(ServiceTask args);
        List<ServiceTask> GetTasks(IServiceSpecification spec);
        void UpdateChanges(ServiceTask task);
    }
}