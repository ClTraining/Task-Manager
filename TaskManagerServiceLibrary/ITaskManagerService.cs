using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Specifications.ClientSpecifications;


namespace TaskManagerServiceLibrary
{
    [ServiceContract]
    public interface ITaskManagerService
    {
        [OperationContract]
        int AddTask(AddTaskArgs task);

        [OperationContract]
        [ServiceKnownType(typeof(ListAllClientSpecification))]
        [ServiceKnownType(typeof(ListByDateClientSpecification))]
        [ServiceKnownType(typeof(ListSingleClientSpecification))]
        List<ClientPackage> GetTasks(IClientSpecification data);

        [OperationContract]
        void Complete(CompleteTaskArgs args);

        [OperationContract]
        void RenameTask(RenameTaskArgs args);

        [OperationContract]
        void SetTaskDueDate(SetDateTaskArgs args);

        [OperationContract]
        void ClearTaskDueDate(ClearDateTaskArgs args);
    }
}