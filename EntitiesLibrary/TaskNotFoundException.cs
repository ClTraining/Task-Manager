using System;

namespace EntitiesLibrary
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(string id) : base(id)
        {
        }
    }
}