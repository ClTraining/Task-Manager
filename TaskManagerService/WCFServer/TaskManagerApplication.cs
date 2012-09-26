using System;
using System.ServiceModel;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerService.TaskManager;
using Xunit;

namespace TaskManagerService.WCFServer
{
    class TaskManagerApplication
    {
        static void Main()
        {
            new TaskManagerService(new ToDoList()).Start();
        }
    }

    public class TaskManagerService : ITaskManagerService
    {
        const string serviceAddress = "net.tcp://localhost:44444";
        private readonly IToDoList tasks;

        public TaskManagerService(IToDoList tasks)
        {
            this.tasks = tasks;
        }

        public ITask AddTask(ITask task)
        {
            return tasks.AddTask(task);
        }

        public void Start()
        {            
            using (var serviceHost = new ServiceHost(typeof(TaskManagerService), new Uri(serviceAddress)))
            {
                serviceHost.Open();
            }
        }
    }


    public class TaskManagerAppTests
    {
        private readonly ITask incomingTask = Substitute.For<ITask>();
        private readonly ITask outgoingTask = Substitute.For<ITask>();
        private readonly IToDoList list = Substitute.For<IToDoList>();
        private readonly ITaskManagerService manager;

        public TaskManagerAppTests()
        {
            manager = new TaskManagerService(list);         
        }

        [Fact]
        public void should_send_and_return_task()
        {
            //arrange
            list.AddTask(outgoingTask).Returns(incomingTask);

            var task = manager.AddTask(outgoingTask);

            task.Should().Be(incomingTask);
        }
    }
}