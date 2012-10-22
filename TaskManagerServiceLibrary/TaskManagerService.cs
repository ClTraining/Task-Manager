using System;
using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Specifications.ClientSpecification;
using Specifications.Mappers;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;
using System.Linq;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly IRepository repository;
        private readonly IToDoList taskList;
        private readonly List<IServiceSpecification> list;

        public TaskManagerService(IRepository repository, IToDoList taskList, List<IServiceSpecification> list)
        {
            this.repository = repository;
            this.list = list;
        }

        public int AddTask(string task)
        {
            return taskList.AddTask(task);
        }

        public List<ContractTask> GetTasks(IClientSpecification specification)
        {
            var spec = list.First(x => x.GetType().Name.StartsWith(specification.GetType().Name));
            return repository.GetTasks();
        }
    }

    public class TaskManagerServiceTests
    {
        private readonly ITaskManagerService service;
        private readonly IToDoList list = Substitute.For<IToDoList>();
        private readonly IRepository repo = Substitute.For<IRepository>();

        public TaskManagerServiceTests()
        {
            service = new TaskManagerService(repo, list);
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
            IClientSpecification spec = new ListSingle(1);
            var task = new ContractTask { Id = 1 };
            list.GetTaskById(1).Returns(task);
            var res = service.GetTasks();
            res.Should().Be(task);
        }

        [Fact]
        public void should_get_all_taasks()
        {
            var listTasks = new List<ContractTask> { new ContractTask { Id = 1, Name = "some", IsCompleted = false } };
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
            var args = new RenameTaskArgs() { Id = 1, Name = "task name" };
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