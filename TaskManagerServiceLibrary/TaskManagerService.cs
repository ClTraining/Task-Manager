using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Specifications.ClientSpecification;
using Specifications.QuerySpecifications;
using TaskManagerServiceLibrary.Repositories;
using Xunit;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly IRepository repository;
        private readonly ISpecificationsConverter converter;

        public TaskManagerService(IRepository repository, ISpecificationsConverter converter)
        {
            this.repository = repository;
            this.converter = converter;
        }

        public List<ContractTask> GetTasks(IClientSpecification input)
        {
            var res = converter.GetQuerySpecification(input);
            res.Initialise(input.Data);
            return repository.GetTasks(res);
        }

        public int AddTask(AddTaskArgs task)
        {
            return repository.AddTask(task);
        }

        public void Complete(CompleteTaskArgs args)
        {
            repository.Complete(args);
        }

        public void RenameTask(RenameTaskArgs args)
        {
            repository.RenameTask(args);
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            repository.SetTaskDueDate(args);
        }

        public void ClearTaskDueDate(ClearDateArgs args)
        {
            repository.ClearTaskDueDate(args);
        }
    }

    public class TaskManagerTests
    {
        private readonly IQuerySpecification qSpec = Substitute.For<IQuerySpecification>();
        private readonly IClientSpecification cSpec = Substitute.For<IClientSpecification>();
        private readonly ISpecificationsConverter converter = Substitute.For<ISpecificationsConverter>();
        private readonly IRepository repo = Substitute.For<IRepository>();

        private readonly TaskManagerService service;

        public TaskManagerTests()
        {
            service = new TaskManagerService(repo, converter);
        }


        [Fact]
        public void should_get_all_tasks()
        {
            var outList = new List<ContractTask>();
            converter.GetQuerySpecification(cSpec).Returns(qSpec);
            repo.GetTasks(qSpec).Returns(outList);

            var contractTasks = service.GetTasks(cSpec);

            contractTasks.Should().BeEquivalentTo(outList);
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
            var args = new ClearDateArgs { Id = 1 };
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
            var args = new SetDateArgs { Id = 1 };
            service.SetTaskDueDate(args);
            repo.Received().SetTaskDueDate(args);
        }
    }
}