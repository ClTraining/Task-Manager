using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using Specifications.ClientSpecification;


namespace TaskManagerServiceLibrary
{
    [ServiceContract]
    public interface ITaskManagerService
    {
        [OperationContract]
        int AddTask(string task);

        [OperationContract]
        List<ContractTask> GetTasks(IClientSpecification specification);
    }
}
