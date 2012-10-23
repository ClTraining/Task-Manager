using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.Arguments.AddTask;
using EntitiesLibrary.Arguments.CompleteTask;
using EntitiesLibrary.Arguments.RenameTask;
using EntitiesLibrary.Arguments.SetDate;
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