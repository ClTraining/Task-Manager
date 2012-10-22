using System.Collections.Generic;
using EntitiesLibrary;
using Specifications.ServiceSpecifications;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(string name);
        List<ContractTask> GetTasks(IServiceSpecification spec);
        void Complete(int id);
        void RenameTask(RenameTaskArgs args);
    }
}
