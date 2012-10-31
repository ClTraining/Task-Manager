using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;

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

        public List<ClientTask> GetTasks(IListCommandArguments input)
        {
            var serviceSpecification = new SpecificationsConverter().GetQuerySpecification(input);
            return todoList.GetTasks(serviceSpecification);
        }

        public void UpdateChanges(IEditCommandArguments args)
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
        private readonly IListCommandArguments cSpec = Substitute.For<IListCommandArguments>();
        private readonly ISpecificationsConverter converter = Substitute.For<ISpecificationsConverter>();
        private readonly IRepository repo = Substitute.For<IRepository>();
        readonly ITodoList todoList = Substitute.For<ITodoList>();

        private readonly ITypeConverter<IListCommandArguments, IServiceSpecification> typeConverter =
            Substitute.For<ITypeConverter<IListCommandArguments, IServiceSpecification>>();

        private readonly TaskManagerService service;

        public TaskManagerTests()
        {
            service = new TaskManagerService(repo, todoList);
        }
    }
}