using System.Collections.Generic;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(AddTaskArgs args);
        List<ServiceTask> GetTasks(IServiceSpecification spec);
        ServiceTask Select(int id);
    }
}