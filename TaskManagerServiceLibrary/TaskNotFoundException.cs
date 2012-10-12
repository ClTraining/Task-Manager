using System;

namespace TaskManagerServiceLibrary
{
    public class TaskNotFoundException : Exception
    {

        public int TaskId { get; set; }
        public TaskNotFoundException(int taskId)
            : base(string.Format("Task not found: (Id = {0})", taskId)) { }
        
        public TaskNotFoundException(string message) : base(message) { }
    }
}
