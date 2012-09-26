using EntitiesLibrary;

namespace TaskManagerApp.DataBaseAccessLayer
{
    public interface IRepository
    {
        ServiceTask SaveTask(ServiceTask task);
    }
}
