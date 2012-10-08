using System;

namespace TaskManagerServiceLibrary
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int id) : base(id.ToString()) { }
        public TaskNotFoundException(string message) : base(message) { }
    }
}
