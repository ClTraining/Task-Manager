using System;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace CommandQueryLibrary.Commands
{
    public class ClearDateServiceCommand : IServiceCommand
    {
        public ServiceTask Update(ICommandArguments args, ServiceTask task)
        {
            task.DueDate = default(DateTime);
            return task;
        }
    }
}