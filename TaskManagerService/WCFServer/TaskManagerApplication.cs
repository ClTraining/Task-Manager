using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerService.TaskManager;
using Xunit;

namespace TaskManagerService.WCFServer
{
    class TaskManagerApplication : ITaskManagerApplication
    {
        private readonly IToDoList _tasks;

        public TaskManagerApplication(IToDoList tasks)
        {
            this._tasks = tasks;
        }

        public ITask AddTask(ITask task)
        {
            return _tasks.AddTask(task);
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