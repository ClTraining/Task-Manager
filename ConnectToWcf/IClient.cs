using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Specifications.ClientSpecifications;

namespace ConnectToWcf
{
    public interface IClient
    {
        int AddTask(AddTaskArgs task);
        List<ClientPackage> GetTasks(IClientSpecification data);
        void UpdateChanges(ICommandArguments args);
    }
}