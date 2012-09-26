using EntitiesLibrary;

namespace TaskManagerService.DataBaseAccessLayer
{
    public interface IRepository
    {
        ServiceTask AddTask(ServiceTask task);
        ServiceTask GetTaskById(int id);
        ServiceTask EditTask(ServiceTask task);
    }
}
