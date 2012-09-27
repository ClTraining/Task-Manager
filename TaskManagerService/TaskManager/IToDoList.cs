<<<<<<< HEAD
﻿#region Using

using System.Collections.Generic;
using EntitiesLibrary;

#endregion

=======
﻿using EntitiesLibrary;
>>>>>>> updated

namespace TaskManagerHost.TaskManager
{
    public interface IToDoList
    {
<<<<<<< HEAD
        ContractTask AddTask(ContractTask task);
        ContractTask GetTaskById(int id);
        List<ContractTask> GetAllTasks();
        ContractTask EditTask(ContractTask task);
=======
        ITask AddTask(ITask task);
>>>>>>> updated
    }
}