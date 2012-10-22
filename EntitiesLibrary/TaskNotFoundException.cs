using System;

namespace TaskManagerServiceLibrary
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(string taskId)
            : base(taskId)
        {
        }

        public string TaskId { get; set; }
    }
}