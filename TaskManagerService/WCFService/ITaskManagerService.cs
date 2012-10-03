using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;

namespace TaskManagerHost.WCFService
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
        void MarkCompleted(int id);

        [OperationContract]
        bool TestConnection();
    }
}
