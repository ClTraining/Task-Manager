using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerService.TaskManager;
using Xunit;

namespace TaskManagerService.WCFServer
{
    class TaskManagerApplication : ITaskManagerApplication
    {
        private readonly IToDoList tasks;

        public TaskManagerApplication(IToDoList tasks)
        {
            this.tasks = tasks;
        }

        public ITask AddTask(ITask task)
        {
            return tasks.AddTask(task);
        }
    }

    public class TaskManagerAppTests
    {
        readonly ITask incomeTask = new ContractTask { Id = 0, Name = "Buy milk" };
        readonly ITask expectedTask = new ServiceTask { Id = 1, Name = "Buy milk" };
        readonly IToDoList tasks = Substitute.For<IToDoList>();

        [Fact]
        public void should_redirect_creating_task_to_todolist()
        {
            var app = new TaskManagerApplication(tasks);

            tasks.AddTask(incomeTask).Returns(expectedTask);

            var resTask = app.AddTask(incomeTask);

            resTask.Should().Be(expectedTask);
        }
    }
}