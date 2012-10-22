using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(string name);
        List<ContractTask> GetTasks();
        void Complete(int id);
        void RenameTask(RenameTaskArgs args);
    }
}
