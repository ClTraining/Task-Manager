using System.Collections.Generic;
using CQRS.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary
{
    public interface ITodoList
    {
        void UpdateChanges(ICommandArguments args);
        int AddTask(AddTaskArgs args);
        List<ClientTask> GetTasks(IServiceSpecification serviceSpecification);
    }
}