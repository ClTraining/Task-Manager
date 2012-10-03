using System;

namespace TaskManagerHost.WCFService
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
        public TaskNotFoundException(string message) : base(message) { }
    }
}
