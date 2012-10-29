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
        [ServiceKnownType(typeof(AddTaskArgs))]
        [ServiceKnownType(typeof(ClearDateTaskArgs))]
        [ServiceKnownType(typeof(CompleteTaskArgs))]
        [ServiceKnownType(typeof(ListTaskArgs))]
        [ServiceKnownType(typeof(RenameTaskArgs))]
        [ServiceKnownType(typeof(SetDateTaskArgs))]
        void UpdateChanges(ICommandArguments args);
    }
}