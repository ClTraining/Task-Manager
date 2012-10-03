using System;

namespace TaskManagerHost.WCFServer
{
    public class TaskNotFoundFault
    {
        public int TaskId{ get; set; }

        public TaskNotFoundFault(int id)
        {
            TaskId = id;
        }
    }

    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int id) : base(id.ToString()) { }
    }
}
