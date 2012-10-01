using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;

namespace TaskManagerHost.WCFServer
{
    [ServiceContract]
    public interface ITaskManagerService
    {
        [OperationContract]
        int AddTask(string task);

        [OperationContract]
        ContractTask GetTaskById(int id);

        [OperationContract]
        List<ContractTask> GetAllTasks();

        [OperationContract]
        bool MarkCompleted(int id);

        [OperationContract]
        bool TestConnection();
    }
}
