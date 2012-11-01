using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary.Commands;
using Xunit;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly ITodoList todoList;
        private readonly IArgToCommandConverter converter;

        public TaskManagerService(ITodoList todoList, IArgToCommandConverter converter)
        {
            this.todoList = todoList;
            this.converter = converter;
        }

        public int AddTask(AddTaskArgs task)
        {
            return todoList.AddTask(task);
        }

        public List<ClientTask> GetTasks(IListCommandArguments input)
        {
            var serviceSpecification = new SpecificationsConverter().GetQuerySpecification(input);
            return todoList.GetTasks(serviceSpecification);
        }

        public void UpdateChanges(IEditCommandArguments args)
        {
//            converter.GetServiceCommand(args).ExecuteCommand();
        }
    }
    
    public class TaskManagerTests
    {
        readonly ITodoList todoList = Substitute.For<ITodoList>();
        private readonly IArgToCommandConverter converter = Substitute.For<IArgToCommandConverter>();

        private readonly TaskManagerService service;

        public TaskManagerTests()
        {
            service = new TaskManagerService(todoList, converter);
        }

        [Fact]
        public void should_add_task_return_id()
        {
            var args = new AddTaskArgs();
            todoList.AddTask(args).Returns(1);

            var result = service.AddTask(args);

            result.Should().Be(1);
        }
    }
}