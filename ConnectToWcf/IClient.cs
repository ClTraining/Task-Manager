using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace ConnectToWcf
{
    public interface IClient
    {
        int AddTask(AddTaskArgs task);
        List<ClientPackage> GetTasks(IListCommandArguments data);
        void UpdateChanges(IEditCommandArguments args);
    }
}