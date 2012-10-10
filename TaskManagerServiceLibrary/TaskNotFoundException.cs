using System;

namespace TaskManagerServiceLibrary
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException() : base("Task list empty") { }
        public TaskNotFoundException(int id) : base(string.Format("Task not found: (Id = {0})", id)) { }
    }
}
