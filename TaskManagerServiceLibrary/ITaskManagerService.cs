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
        void MarkTaskAsCompleted(CompleteTaskArgs id);

        [OperationContract]
        void SetTaskDueDate(SetDateArgs args);
    }
}