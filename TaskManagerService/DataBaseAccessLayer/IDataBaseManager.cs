using EntitiesLibrary;

namespace TaskManagerService.DataBaseAccessLayer
{
    public interface IDataBaseManager
    {
        ServiceTask SaveTask(ServiceTask task);
        int GetId();
    }
}
