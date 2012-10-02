using System;

namespace TaskManagerHost.WCFServer
{
    public class TaskNotFoundException : Exception
    {
        public int Id{ get; private set; }

        public TaskNotFoundException(int id)
        {
            Id = id;
        }
    }
}
