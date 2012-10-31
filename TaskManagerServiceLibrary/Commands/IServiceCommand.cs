using TaskManagerServiceLibrary.Repositories;

namespace TaskManagerServiceLibrary.Commands
{
    public interface IServiceCommand
    {
        void ExecuteCommand(IRepository repo);
    }
}