using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;

namespace TaskManagerHost.WCFServer
{
    [ServiceContract]
    public interface ITaskManagerService
    {
        [OperationContract]
        ContractTask AddTask(ContractTask task);

        [OperationContract]
        ContractTask GetTaskById(int id);

        [OperationContract]
        List<ContractTask> GetAllTasks();

        [OperationContract]
        ContractTask Edit(ContractTask task);
    }
}
