using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerService.DataBaseAccessLayer;
using Xunit;

namespace TaskManagerService.TaskManager
{
    public class ToDoList : IToDoList
    {
        public ITask AddTask(ITask task)
        {
            return null;
        }
    }
}
