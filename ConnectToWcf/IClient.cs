using System.Collections.Generic;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace ConnectToWcf
{
    public interface IClient
    {
        int AddTask(AddTaskArgs task);
        List<ClientTask> GetTasks(IListCommandArguments args);
        void ExecuteCommand(IEditCommandArguments args);
    }
}