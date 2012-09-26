using System;
using System.ServiceModel;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerApp.TaskManager;
using TaskManagerHost.DataBaseAccessLayer;
using TaskManagerHost.TaskManager;
using Xunit;

namespace TaskManagerHost.WCFServer
{
    static class TaskManagerApplication
    {
        static void Main()
        {
            const string address = "net.tcp://localhost:44444";
            using (var serviceHost = new ServiceHost(typeof(TaskManagerService), new Uri(address)))
            {
                serviceHost.Open();
                Console.WriteLine("Host started");
                Console.WriteLine("Press Enter to terminate the host...");
                Console.ReadLine();
            }
        }
    }

    public class TaskManagerService : ITaskManagerService
    {
        private readonly IToDoList tasks;

        public TaskManagerService() {
            Console.WriteLine("added new task");
            tasks = new ToDoList(new TaskFactory(), new MemoRepository());
        }

        public TaskManagerService(IToDoList tasks)
        {
            this.tasks = tasks;
        }

        public ServiceTask AddTask(ContractTask task)
        {
            return tasks.AddTask(task);
        }
    }


    public class TaskManagerServiceTests
    {
        private readonly ServiceTask incomingTask = new ServiceTask();
        private readonly ContractTask outgoingTask = new ContractTask();
        private readonly IToDoList list = Substitute.For<IToDoList>();
        private readonly ITaskManagerService manager;

        public TaskManagerServiceTests()
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