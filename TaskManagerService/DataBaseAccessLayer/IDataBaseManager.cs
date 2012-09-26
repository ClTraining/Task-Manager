using EntitiesLibrary;

namespace TaskManagerService.DataBaseAccessLayer
{
    public interface IRepository
    {
        ServiceTask SaveTask(ServiceTask task);
    }
}
