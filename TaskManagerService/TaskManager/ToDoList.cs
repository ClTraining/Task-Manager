using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerApp.DataBaseAccessLayer;
using TaskManagerHost.TaskManager;
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
