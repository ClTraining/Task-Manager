using System.Collections.Generic;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary
{
    public interface ITodoList
    {
        void UpdateChanges(IEditCommandArguments args);
        int AddTask(AddTaskArgs args);
        List<ClientTask> GetTasks(IServiceSpecification serviceSpecification);
    }
}