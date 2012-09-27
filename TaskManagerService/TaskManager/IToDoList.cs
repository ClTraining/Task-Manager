using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerHost.TaskManager
{
    public interface IToDoList
    {
        ServiceTask AddTask(ContractTask task);
    }
}