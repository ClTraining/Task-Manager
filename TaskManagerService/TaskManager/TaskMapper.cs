#region Using

using EntitiesLibrary;

#endregion


namespace TaskManagerHost.TaskManager
{
    class TaskMapper:ITaskMapper
    {
        public ServiceTask ConvertToService(ContractTask task)
        {
            var newTask = new ServiceTask {Id = task.Id, Name = task.Name, IsCompleted = task.IsCompleted};
            return newTask;
        }

        public ContractTask ConvertToContract(ServiceTask task)
        {
            var newTask = new ContractTask { Id = task.Id, Name = task.Name, IsCompleted = task.IsCompleted };
            return newTask;
        }
    }
}
