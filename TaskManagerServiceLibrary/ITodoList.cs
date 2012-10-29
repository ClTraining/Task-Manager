using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Specifications.ServiceSpecifications;

namespace TaskManagerServiceLibrary
{
    public interface ITodoList
    {
        void UpdateChanges(ICommandArguments args);
        int AddTask(AddTaskArgs args);
        List<ClientTask> GetTasks(IServiceSpecification serviceSpecification);
    }
}