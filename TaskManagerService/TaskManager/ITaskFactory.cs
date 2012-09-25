﻿#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerServise.Entities;

#endregion


namespace TaskManagerServise.TaskManager
{
    public interface ITaskFactory
    {
        CTask Create();
    }
}
