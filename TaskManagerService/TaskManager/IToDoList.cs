using EntitiesLibrary;


namespace TaskManagerService.TaskManager
{
    public interface IToDoList
    {
        ContractTask AddTask(ContractTask task);
    }
}