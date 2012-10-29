using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using CQRS.ClientSpecifications;
using CQRS.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly ITodoList todoList;

        public TaskManagerService(IRepository repo, ITodoList todoList)
        {
            this.todoList = todoList;
        }

        public List<ClientTask> GetTasks(IClientSpecification specification)
        {
            var serviceSpecification = new SpecificationsConverter().GetQuerySpecification(specification);
            return todoList.GetTasks(serviceSpecification);
        }

        public void UpdateChanges(ICommandArguments args)
        {
            todoList.UpdateChanges(args);
        }

        public int AddTask(AddTaskArgs task)
        {
            return todoList.AddTask(task);
        }
    }

    public class TaskManagerTests
    {
        private readonly IServiceSpecification qSpec = Substitute.For<IServiceSpecification>();
        private readonly IClientSpecification cSpec = Substitute.For<IClientSpecification>();
        private readonly ISpecificationsConverter converter = Substitute.For<ISpecificationsConverter>();
        private readonly IRepository repo = Substitute.For<IRepository>();
        readonly ITodoList todoList = Substitute.For<ITodoList>();

        private readonly ITypeConverter<IClientSpecification, IServiceSpecification> typeConverter =
            Substitute.For<ITypeConverter<IClientSpecification, IServiceSpecification>>();

        private readonly TaskManagerService service;

        public TaskManagerTests()
        {
            service = new TaskManagerService(repo, todoList);
        }
    }
}