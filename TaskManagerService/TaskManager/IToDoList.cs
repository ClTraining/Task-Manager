
﻿#region Using

using System.Collections.Generic;
using EntitiesLibrary;

#endregion


namespace TaskManagerHost.TaskManager
{
    public interface IToDoList
    {
        int AddTask(string name);
        ContractTask GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        bool MarkCompleted(int id);
    }
}