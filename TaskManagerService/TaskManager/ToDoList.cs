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
        private ITask incomingTask = new ContractTask {Id = 0, Name = "Buy cheese"};
        private ITask expectedTask = new ServiceTask {Id = 1, Name = "Buy cheese"};

        [Fact]
        public void todolist_asks_factory_for_new_task_and_saves_list()
        {
            
        }
    }
}
