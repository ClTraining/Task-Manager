using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerService.TaskManager
{
    public class ToDoList : IToDoList
    {
        public ContractTask AddTask(ContractTask task)
        {
            return new ServiceTask();
        }
    }

    public class ToDoListTests
    {
        ITaskFactory factory = Substitute.For<ITaskFactory>();

        [Fact]
        public void should_create_task_in_factory_and_send_to_repository()
        {
            var task = factory.Create();
            task.Should().Be(new ServiceTask());
        }
    }
}
