using System;

namespace TaskManagerService.WCFService
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int id) : base(id.ToString()) { }
        public TaskNotFoundException(string message) : base(message) { }
    }
}
