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

        [FaultContract(typeof(TaskNotFoundException))]
        [OperationContract]
        ContractTask GetTaskById(int id);

        [OperationContract]
        List<ContractTask> GetAllTasks();

        [FaultContract(typeof(TaskNotFoundException))]
        [OperationContract]
        void MarkCompleted(int id);

        [OperationContract]
        bool TestConnection();
    }
}
