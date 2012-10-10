using System;

namespace TaskManagerServiceLibrary
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int taskId):base(string.Format("Task not found: (Id = {0})", taskId))
        {
        }
    }
}
