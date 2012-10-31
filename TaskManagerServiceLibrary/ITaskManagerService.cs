using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;


namespace TaskManagerServiceLibrary
{
    [ServiceContract]
    public interface ITaskManagerService
    {
        [OperationContract]
        int AddTask(AddTaskArgs task);

        [OperationContract]
        [ServiceKnownType(typeof(ListAllTaskArgs))]
        [ServiceKnownType(typeof(ListByDateTaskArgs))]
        [ServiceKnownType(typeof(ListSingleTaskArgs))]
        List<ClientTask> GetTasks(IListCommandArguments data);

        [OperationContract]
        [ServiceKnownType(typeof(ClearDateTaskArgs))]
        [ServiceKnownType(typeof(CompleteTaskArgs))]
        [ServiceKnownType(typeof(RenameTaskArgs))]
        [ServiceKnownType(typeof(SetDateTaskArgs))]
        void UpdateChanges(IEditCommandArguments args);
    }
}