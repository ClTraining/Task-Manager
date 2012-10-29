using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace CQRS.Commands
{
    public class RenameServiceCommand : IServiceCommand
    {
        public ServiceTask Update(ICommandArguments args, ServiceTask task)
        {
            task.Name = ((RenameTaskArgs)args).Name;
            return task;
        }
    }
}