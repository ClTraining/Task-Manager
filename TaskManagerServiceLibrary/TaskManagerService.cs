using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly IToDoList taskList;

        public TaskManagerService(IToDoList list)
        {
            taskList = list;
        }

        public int AddTask(string task)
        {
            return taskList.AddTask(task);
        }

        public ContractTask GetTaskById(int id)
        {
            return taskList.GetTaskById(id);
        }

        public List<ContractTask> GetAllTasks()
        {
            return taskList.GetAllTasks();
        }

        public void Complete(int id)
        {
            taskList.Complete(id);
        }

        public bool TestConnection()
        {
            return true;
        }

        public void RenameTask(RenameTaskArgs args)
        {
            taskList.RenameTask(args);
        }
    }

    public class TaskManagerServiceTests
    {
        private readonly ITaskManagerService service;
        private readonly IToDoList list = Substitute.For<IToDoList>();

        public TaskManagerServiceTests()
        {
            service = new TaskManagerService(list);
        }

        [Fact]
        public void should_create_task_and_return_taskid()
        {
            list.AddTask("some task").Returns(1);
            var res = service.AddTask("some task");
            res.Should().Be(1);
        }

        [Fact]
        public void should_get_task_by_id_and_return_task()
        {
            var task = new ContractTask {Id = 1};
            list.GetTaskById(1).Returns(task);
            var res = service.GetTaskById(1);
            res.Should().Be(task);
        }

        [Fact]
        public void should_get_all_taasks()
        {
            var listTasks = new List<ContractTask> {new ContractTask {Id = 1, Name = "some", IsCompleted = false}};
            list.GetAllTasks().Returns(listTasks);
            var res = service.GetAllTasks();
            res.Should().BeEquivalentTo(listTasks);
        }

        [Fact]
        public void should_send_id_receive_completed_value()
        {
            service.Complete(1);
            list.Received().Complete(1);
        }

        [Fact]
        public void should_send_rename_task()
        {
            var args = new RenameTaskArgs() {Id = 1, Name = "task name"};
            service.RenameTask(args);
            list.Received().RenameTask(args);
        }

        [Fact]
        public void test_connection_should_return_always_true()
        {
            var result = service.TestConnection();
            result.Should().Be(true);
        }
    }
}