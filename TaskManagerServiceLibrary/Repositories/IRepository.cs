using System.Collections.Generic;
using EntitiesLibrary;
using TaskManagerServiceLibrary.Specifications;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(string name);
        ServiceTask GetTaskById(int id);
        List<ServiceTask> GetAllTasks();
        List<ContractTask> GetTasks(ISpecification spec);
        void Complete(int id);
        void RenameTask(RenameTaskArgs args);
    }
}
