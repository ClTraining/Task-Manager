using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.Arguments.AddTask;
using EntitiesLibrary.Arguments.CompleteTask;
using EntitiesLibrary.Arguments.RenameTask;
using EntitiesLibrary.Arguments.SetDate;


namespace TaskManagerServiceLibrary
{
    [ServiceContract]
    public interface ITaskManagerService
    {
        [OperationContract]
        int AddTask(AddTaskArgs task);

        [OperationContract]
        List<ContractTask> GetTasks(DataPackage pack);

        [OperationContract]
        void Complete(CompleteTaskArgs args);

        [OperationContract]
        void RenameTask(RenameTaskArgs args);

        [OperationContract]
        void SetTaskDueDate(SetDateArgs args);
    }
}