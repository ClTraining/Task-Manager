using System.Collections.Generic;
using CommandQueryLibrary.ClientSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace ConnectToWcf
{
    public interface IClient
    {
        int AddTask(AddTaskArgs task);
        List<ClientTask> GetTasks(IClientSpecification data);
        void ExecuteCommand(ICommandArguments args);
    }
}