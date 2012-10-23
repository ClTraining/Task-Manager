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
        ContractTask GetTaskById(int id);

        [OperationContract]
        List<ContractTask> GetAllTasks();

        [OperationContract]
        void MarkTaskAsCompleted(CompleteTaskArgs id);

        [OperationContract]
        bool TestConnection();

        [OperationContract]
        void RenameTask(RenameTaskArgs args);

        [OperationContract]
        void SetTaskDueDate(SetDateArgs args);
    }
}