using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
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

        public List<ClientPackage> GetTasks(IListCommandArguments input)
        {
            var specification = new SpecificationsConverter().GetQuerySpecification(input);
            return repository.GetTasks(specification);
        }

        public void UpdateChanges(IEditCommandArguments args)
        {
            repository.UpdateChanges(args);
        }

        public int AddTask(AddTaskArgs task)
        {
            var addTask = repository.AddTask(task);

            return addTask;
        }
    }

    public class TaskManagerTests
    {
        private readonly IServiceSpecification qSpec = Substitute.For<IServiceSpecification>();
        private readonly IListCommandArguments cSpec = Substitute.For<IListCommandArguments>();
        private readonly ISpecificationsConverter converter = Substitute.For<ISpecificationsConverter>();
        private readonly IRepository repo = Substitute.For<IRepository>();

        private readonly ITypeConverter<IListCommandArguments, IServiceSpecification> typeConverter =
            Substitute.For<ITypeConverter<IListCommandArguments, IServiceSpecification>>();

        private readonly TaskManagerService service;

        public TaskManagerTests()
        {
            service = new TaskManagerService(repo);
        }

        [Fact]
        public void should_add_task_and_return_id()
        {
            var args = new AddTaskArgs();
            service.AddTask(args);
            repo.Received().AddTask(args);
        }
    }
}