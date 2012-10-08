using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(string name);
        ServiceTask GetTaskById(int id);
        List<ServiceTask> GetAllTasks();
        void Complete(int id);
    }
}
