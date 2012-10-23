using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
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

        #region ITaskManagerService Members

        public int AddTask(AddTaskArgs task)
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

        public void MarkTaskAsCompleted(CompleteTaskArgs args)
        {
            taskList.MarkTaskAsCompleted(args);
        }

        public bool TestConnection()
        {
            return true;
        }

        public void RenameTask(RenameTaskArgs args)
        {
            taskList.RenameTask(args);
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            taskList.SetTaskDueDate(args);
        }

        public void ClearTaskDueDate(ClearDateArgs args)
        {
            taskList.ClearTaskDueDate(args);
        }

        #endregion
    }

    public class TaskManagerServiceTests
    {
        private readonly IToDoList list = Substitute.For<IToDoList>();
        private readonly ITaskManagerService service;

        public TaskManagerServiceTests()
        {
            service = new TaskManagerService(list);
        }

        [Fact]
        public void should_create_task_and_return_taskid()
        {
            var addTaskArgs = new AddTaskArgs {Name = "some task"};
            list.AddTask(addTaskArgs).Returns(1);
            var res = service.AddTask(addTaskArgs);
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
            var completeTaskArgs = new CompleteTaskArgs {Id = 1};
            service.MarkTaskAsCompleted(completeTaskArgs);
            list.Received().MarkTaskAsCompleted(completeTaskArgs);
        }

        [Fact]
        public void should_send_rename_task()
        {
            var args = new RenameTaskArgs {Id = 1, Name = "task name"};
            service.RenameTask(args);
            list.Received().RenameTask(args);
        }

        [Fact]
        public void test_connection_should_return_always_true()
        {
            var result = service.TestConnection();
            result.Should().Be(true);
        }

        [Fact]
        public void should_send_set_date_for_task()
        {
            var dateTime = DateTime.Now;
            var args = new SetDateArgs {Id = 1, DueDate = dateTime};
            service.SetTaskDueDate(args);
            list.Received().SetTaskDueDate(args);
        }

        [Fact]
        public void should_send_clear_date_for_task()
        {
            var dateTime = DateTime.Now;
            var args = new ClearDateArgs { Id = 1};
            service.ClearTaskDueDate(args);
            list.Received().ClearTaskDueDate(args);
        }
    }
}