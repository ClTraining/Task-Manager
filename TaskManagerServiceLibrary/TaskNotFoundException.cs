using System;

namespace TaskManagerServiceLibrary
{
    public class TaskNotFoundException : Exception
    {
        public string TaskId { get; set; }

        public TaskNotFoundException(string taskId)
            : base(taskId) { }
    }
}