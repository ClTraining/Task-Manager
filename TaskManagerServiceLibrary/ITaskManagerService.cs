using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using TaskManagerServiceLibrary.Specifications;

namespace TaskManagerServiceLibrary
{
    [ServiceContract]
    public interface ITaskManagerService
    {
        [OperationContract]
        int AddTask(string task);

        //[OperationContract]
        //ContractTask GetTaskById(int id);

        //[OperationContract]
        //List<ContractTask> GetAllTasks();

        [OperationContract]
        List<ContractTask> GetTasks(int? id);
        
        //[OperationContract]
        //void Complete(int id);

        //[OperationContract]
        //bool TestConnection();

        //[OperationContract]
        //void RenameTask(RenameTaskArgs args);
    }
}
