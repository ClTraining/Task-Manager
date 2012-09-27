#region Using

using EntitiesLibrary;

#endregion


namespace TaskManagerHost.TaskManager
{
    public interface ITaskFactory
    {
        ServiceTask Create();
    }
}
