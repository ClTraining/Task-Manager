using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerHost.DataBaseAccessLayer
{
    public interface IRepository
    {
        int AddTask(string name);
        ServiceTask GetTaskById(int id);
        List<ServiceTask> GetAllTasks();
        bool MarkCompleted(int id);
        bool DeleteAllTasks();
    }
}
