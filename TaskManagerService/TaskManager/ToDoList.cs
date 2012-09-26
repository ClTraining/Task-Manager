using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerApp.DataBaseAccessLayer;
using Xunit;

namespace TaskManagerApp.TaskManager
{
    public class ToDoList : IToDoList
    {
        public ITask AddTask(ITask task)
        {
            return null;
        }
    }
}
