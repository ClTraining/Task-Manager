using System;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace CQRS.Commands
{
    public class SetDateServiceCommand : IServiceCommand
    {
        public ServiceTask Update(ICommandArguments args, ServiceTask task)
        {
            if(!task.IsCompleted)
                task.DueDate = ((SetDateTaskArgs) args).DueDate;
            else throw new Exception("Task is completed");

            return task;
        }
    }
}