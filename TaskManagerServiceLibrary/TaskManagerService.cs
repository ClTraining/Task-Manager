using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Specifications.ClientSpecification;
using Specifications.QuerySpecifications;
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
        private readonly ISpecificationsConverter converter;

        public TaskManagerService(IRepository repository, List<IQuerySpecification> list, ISpecificationsConverter converter)
        {
            this.repository = repository;
            this.converter = converter;
        }

        public int AddTask(AddTaskArgs task)
        {
            return repository.AddTask(task);
        }

        public List<ContractTask> GetTasks(IClientSpecification input)
        {
            var res = converter.GetQuerySpecification(input);
            res.Initialise(input.Data);
            return repository.GetTasks(res);
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
        private readonly IQuerySpecification spec = Substitute.For<IQuerySpecification>();
        private readonly IRepository repo = Substitute.For<IRepository>();

        readonly ITaskManagerService service;

        public TaskManagerTests()
        {
            specs.Add(spec);
            service = new TaskManagerService(repo, specs);
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var clientSpec = Substitute.For<IClientSpecification>();
            clientSpec.GetType().Name.ReturnsForAnyArgs("aa");

            //var cSpec = Substitute.For<IClientSpecification>();
            //var serviceTask = new ServiceTask();
            //var contractTasks = new List<ContractTask> { new ContractTask() };
            //spec.IsSatisfied(serviceTask).Returns(true);
            //repo.GetTasks(spec).Returns(contractTasks);
            //spec.Initialise();
            //var result = service.GetTasks(cSpec);

            //result.Should().BeEquivalentTo(contractTasks);
        }

        [Fact]
        public void should_send_clear_date_for_task()
        {
            var args = new ClearDateArgs {Id = 1};
            service.ClearTaskDueDate(args);
            repo.Received().ClearTaskDueDate(args);
        }
    }
}