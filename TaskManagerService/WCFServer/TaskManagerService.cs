using System;
using System.Collections.Generic;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerHost.TaskManager;
using Xunit;

namespace TaskManagerHost.WCFServer
{
    public class TaskManagerService : ITaskManagerService
    {
        public IToDoList taskList;

        public TaskManagerService(IToDoList list)
        {
            taskList = list;

            Console.WriteLine("new request added");
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

        public bool MarkCompleted(int id)
        {
            return taskList.MarkCompleted(id);
        }

        public bool TestConnection()
        {
            return true;
        }
    }

    public class TaskManagerServiceTests
    {
        readonly ITaskManagerService service = Substitute.For<ITaskManagerService>();
        readonly List<ContractTask> list = new List<ContractTask>{new ContractTask { Name = "some", Id = 1 }};

        //[Fact]
        //public void should_create_task_and_return_taskid()
        //{
        //    const string taskName = "some id";
        //    service.taskList.AddTask(taskName).Returns(1);
        //    var res = service.AddTask(taskName);
        //    res.Should().Be(1);
        //}

        //[Fact]
        //public void should_get_task_by_id_and_return_task()
        //{
        //    service.taskList.GetTaskById(1).Returns(list[0]);
        //    var res = service.GetTaskById(1);
        //    res.Should().Be(list[0]);
        //}

        //[Fact]
        //public void should_get_all_taasks()
        //{
        //    service.taskList.GetAllTasks().Returns(list);
        //    var res = service.GetAllTasks();
        //    res.Should().BeEquivalentTo(list);
        //}

        //[Fact]
        //public void should_send_id_receive_completed_value()
        //{
        //    service.taskList.MarkCompleted(1).Returns(true);
        //    var res = service.MarkCompleted(1);
        //    res.Should().Be(true);
        //}
    }
}