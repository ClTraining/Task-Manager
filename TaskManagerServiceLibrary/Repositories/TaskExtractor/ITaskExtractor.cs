using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Repositories.TaskExtractor
{
    public interface ITaskExtractor
    {
        ServiceTask SelectTaskById(int id);
    }
}
