using System;
using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Specifications.ClientSpecifications;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.Repositories;
using Xunit;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly IRepository repository;

        public TaskManagerService(IRepository repo)
        {
            repository = repo;
        }

        public List<ClientPackage> GetTasks(IClientSpecification input)
        {
            var specification = new SpecificationsConverter().GetQuerySpecification(input);
            return repository.GetTasks(specification);
        }

        public int AddTask(AddTaskArgs task)
        {
            var addTask = repository.AddTask(task);

            return addTask;
        }

        public void Complete(CompleteTaskArgs args)
        {
            repository.Complete(args);
        }

        public void RenameTask(RenameTaskArgs args)
        {
            repository.RenameTask(args);
        }

        public void SetTaskDueDate(SetDateTaskArgs args)
        {
            repository.SetTaskDueDate(args);
        }

        public void ClearTaskDueDate(ClearDateTaskArgs args)
        {
            repository.ClearTaskDueDate(args);
        }
    }

    public class TaskManagerTests
    {
        private readonly IServiceSpecification qSpec = Substitute.For<IServiceSpecification>();
        private readonly IClientSpecification cSpec = Substitute.For<IClientSpecification>();
        private readonly ISpecificationsConverter converter = Substitute.For<ISpecificationsConverter>();
        private readonly IRepository repo = Substitute.For<IRepository>();

        private readonly ITypeConverter<IClientSpecification, IServiceSpecification> typeConverter =
            Substitute.For<ITypeConverter<IClientSpecification, IServiceSpecification>>();

        private readonly TaskManagerService service;

        public TaskManagerTests()
        {
            service = new TaskManagerService(repo);
        }


        [Fact]
        public void should_get_tasks()
        {
            var outList = new List<ClientPackage> { new ClientPackage { Id = 1 } };
            converter.GetQuerySpecification(cSpec).Returns(qSpec);
            repo.GetTasks(qSpec).Returns(outList);
            
            var contractTasks = service.GetTasks(cSpec);

            contractTasks.Should().BeEquivalentTo(outList);
        }

        [Fact]
        public void should_throw_exception_if_no_task()
        {
            var outList = new List<ClientPackage>();
            converter.GetQuerySpecification(cSpec).Returns(qSpec);
            repo.GetTasks(qSpec).Returns(outList);

            Action action = () => service.GetTasks(cSpec);

            action.ShouldThrow<TaskNotFoundException>();
        }

        [Fact]
        public void should_add_task_and_return_id()
        {
            var args = new AddTaskArgs();
            service.AddTask(args);
            repo.Received().AddTask(args);
        }

        [Fact]
        public void should_send_clear_date_for_task()
        {
            var args = new ClearDateTaskArgs { Id = 1 };
            service.ClearTaskDueDate(args);
            repo.Received().ClearTaskDueDate(args);
        }

        [Fact]
        public void should_mard_task_as_completed()
        {
            var args = new CompleteTaskArgs { Id = 1 };
            service.Complete(args);
            repo.Received().Complete(args);
        }
        [Fact]
        public void should_rename_task()
        {
            var args = new RenameTaskArgs { Id = 1 };
            service.RenameTask(args);
            repo.Received().RenameTask(args);
        }

        [Fact]
        public void should_set_task_due_date()
        {
            var args = new SetDateTaskArgs { Id = 1 };
            service.SetTaskDueDate(args);
            repo.Received().SetTaskDueDate(args);
        }
    }
}