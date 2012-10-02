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
        [FaultContract(typeof(TaskNotFoundException))]
        ContractTask GetTaskById(int id);

        [OperationContract]
        List<ContractTask> GetAllTasks();

        [OperationContract]
        [FaultContract(typeof(TaskNotFoundException))]
        void MarkCompleted(int id);

        [OperationContract]
        bool TestConnection();
    }
}
