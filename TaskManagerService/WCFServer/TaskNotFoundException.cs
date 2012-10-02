using System;

namespace TaskManagerHost.WCFServer
{
    public class TaskNotFoundException : Exception
    {
        public int TaskId{ get; private set; }

        public TaskNotFoundException(int id)
        {
            TaskId = id;
        }
    }
}
