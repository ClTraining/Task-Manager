#region Using

using System;

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
