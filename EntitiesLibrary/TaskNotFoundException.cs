using System;

namespace EntitiesLibrary
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int id) : base(id.ToString())
        {
        }
    }
}