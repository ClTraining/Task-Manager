using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace CommandQueryLibrary.Commands
{
    public class CompleteServiceCommand : IServiceCommand
    {
        public ServiceTask Update(ICommandArguments args, ServiceTask task)
        {
            task.IsCompleted = true;
            return task;
        }
    }
}