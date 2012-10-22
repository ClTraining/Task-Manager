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
        int AddTask(AddTaskArgs task);

        [OperationContract]
        List<ContractTask> GetTasks(IClientSpecification specification);

        [OperationContract]
        void Complete(CompleteTaskArgs args);

        [OperationContract]
        void RenameTask(RenameTaskArgs args);

        [OperationContract]
        void SetTaskDueDate(SetDateArgs args);
    }
}