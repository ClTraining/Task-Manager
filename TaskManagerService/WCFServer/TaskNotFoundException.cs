#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace TaskManagerHost.WCFServer
{
    public class TaskNotFoundException: Exception
    {
        public int TaskId{ get; private set; }

        public TaskNotFoundException(int id)
            : base()
        {
            TaskId = id;
        }
    }
}
