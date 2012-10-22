using System.Collections.Generic;
using EntitiesLibrary;
using Specifications.ClientSpecification;

namespace ConnectToWcf
{
    public interface IClientConnection
    {
        int AddTask(string task);
        List<ContractTask> GetTasks(IClientSpecification id);
        
        //List<ContractTask> GetTaskById(int id);
        //List<ContractTask> GetAllTasks();
        //void Complete(int id);
        //void RenameTask(RenameTaskArgs args);
    }
}