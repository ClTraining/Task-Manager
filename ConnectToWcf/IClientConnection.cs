using System.Collections.Generic;
using EntitiesLibrary;
using Specifications.ClientSpecification;

namespace ConnectToWcf
{
    public interface IClientConnection
    {
        int AddTask(string task);
        List<ContractTask> GetTasks(IClientSpecification id);
        
        void RenameTask(RenameTaskArgs args);
        void Complete(int input);
    }
}