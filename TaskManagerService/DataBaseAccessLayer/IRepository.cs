using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerHost.DataBaseAccessLayer
{
    public interface IRepository
    {
        ServiceTask AddTask(ServiceTask task);
        ServiceTask GetTaskById(int id);
        List<ServiceTask> GetAllTasks();
        ServiceTask EditTask(ServiceTask task);
    }
}
