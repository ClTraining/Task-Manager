namespace TaskManagerServiceLibrary.Commands
{
    public interface IServiceCommand<in T>
    {
        void ExecuteCommand(T args);
    }
}