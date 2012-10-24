using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Specifications.ClientSpecification;


namespace TaskManagerServiceLibrary
{
    [ServiceContract]
    public interface ITaskManagerService
    {
        [OperationContract]
        int AddTask(AddTaskArgs task);

        [OperationContract]
        [ServiceKnownType(typeof(ListAll))]
        [ServiceKnownType(typeof(ListByDate))]
        [ServiceKnownType(typeof(ListSingle))]
        List<ContractTask> GetTasks(IClientSpecification data);

        [OperationContract]
        void Complete(CompleteTaskArgs args);

        [OperationContract]
        void RenameTask(RenameTaskArgs args);

        [OperationContract]
        void SetTaskDueDate(SetDateArgs args);

        [OperationContract]
        void ClearTaskDueDate(ClearDateArgs args);
    }
}