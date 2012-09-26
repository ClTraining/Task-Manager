using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerService.TaskManager
{
    public class ToDoList : IToDoList
    {
        public ITask AddTask(ITask task)
        {
            return new ServiceTask();
        }
    }

    public class ToDoListTests
    {
    }
}
