using System.Collections.Generic;
using System.ServiceModel;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary.Commands;
using TaskManagerServiceLibrary.Converters;
using TaskManagerServiceLibrary.ToDoList;
using Xunit;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly ITodoList todoList;
        private readonly IArgToCommandConverter commandConverter;
        private readonly ISpecificationsConverter specConverter;

        public TaskManagerService(ITodoList todoList, IArgToCommandConverter commandConverter, ISpecificationsConverter specConverter)
        {
            this.todoList = todoList;
            this.commandConverter = commandConverter;
            this.specConverter = specConverter;
        }

        public int AddTask(AddTaskArgs task)
        {
            return todoList.AddTask(task);
        }

        public List<ClientTask> GetTasks(IListCommandArguments args)
        {
            return todoList.GetTasks(specConverter.GetQuerySpecification(args));
        }

        public void UpdateChanges(IEditCommandArguments args)
        {
            commandConverter.GetServiceCommand(args).ExecuteCommand();
        }
    }
    
    public class TaskManagerTests
    {
        readonly ITodoList todoList = Substitute.For<ITodoList>();
        private readonly IArgToCommandConverter comConverter = Substitute.For<IArgToCommandConverter>();
        private readonly ISpecificationsConverter specConverter = Substitute.For<ISpecificationsConverter>();

        private readonly TaskManagerService service;

        public TaskManagerTests()
        {
            service = new TaskManagerService(todoList, comConverter, specConverter);
        }

        [Fact]
        public void should_add_task_return_id()
        {
            var args = new AddTaskArgs();
            todoList.AddTask(args).Returns(1);

            var result = service.AddTask(args);

            result.Should().Be(1);
        }

        [Fact]
        public void should_get_all_tasks_from_server()
        {
            var args = new ListAllTaskArgs();
            var spec = new ListAllServiceSpecification();
            var tasks = new List<ClientTask>();

            specConverter.GetQuerySpecification(args).Returns(spec);
            todoList.GetTasks(spec).Returns(tasks);
            
            var result = service.GetTasks(args);
            
            result.Should().BeEquivalentTo(tasks);
        }

        [Fact]
        public void should_update_changes_on_server()
        {
            var args = new CompleteTaskArgs();
            const int id = 1;
            var command = new CompleteServiceCommand(todoList){Id = id};

            comConverter.GetServiceCommand(args).Returns(command);

            service.UpdateChanges(args);

            command.ExecuteCommand();
        }
    }
}