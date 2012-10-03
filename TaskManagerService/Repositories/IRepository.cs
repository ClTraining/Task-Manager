using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerHost.Repositories
{
    public interface IRepository
    {
        int AddTask(string name);
        ServiceTask GetTaskById(int id);
        List<ServiceTask> GetAllTasks();
        void MarkCompleted(int id);
    }
}
