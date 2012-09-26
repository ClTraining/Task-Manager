using EntitiesLibrary;

namespace TaskManagerApp.DataBaseAccessLayer
{
    public interface IRepository
    {
        ServiceTask AddTask(ServiceTask task);
        ServiceTask GetTaskById(int id);
        ServiceTask EditTask(ServiceTask task);
    }
}
