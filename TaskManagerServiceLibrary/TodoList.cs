using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary
{
    public class TodoList : ITodoList
    {
        private readonly IRepository repo;
        private readonly ITaskMapper mapper;

        public TodoList(IRepository repo, ITaskMapper mapper, IArgToCommandConverter converter)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public int AddTask(AddTaskArgs args)
        {
            return repo.AddTask(args);
        }

        public List<ClientTask> GetTasks(IServiceSpecification serviceSpecification)
        {
            return repo.GetTasks(serviceSpecification)
                .Select(mapper.ConvertToClient)
                .ToList();
        }

        public void ClearDate(int id)
        {
            var task = repo.Select(id);
            task.DueDate = default(DateTime);
            repo.UpdateChanges(task);
        }

        public void CompleteTask(int id)
        {
            var task = repo.Select(id);
            task.IsCompleted = true;
            repo.UpdateChanges(task);
        }

        public void RenameTask(int id, string newName)
        {
            var task = repo.Select(id);
            task.Name = newName;
            repo.UpdateChanges(task);
        }

        public void SetTaskDate(int id, DateTime dueDate)
        {
            var task = repo.Select(id);
            if (task.IsCompleted) throw new CouldNotSetDateException("Could not set date to completed task.");
            task.DueDate = dueDate;
            repo.UpdateChanges(task);
        }
    }

    public class TodoListTests
    {
        private readonly IRepository repo = Substitute.For<IRepository>();
        private readonly ITaskMapper mapper = Substitute.For<ITaskMapper>();
        private readonly IArgToCommandConverter converter = Substitute.For<IArgToCommandConverter>();
        readonly TodoList todoList;
        const int id = 1;

        public TodoListTests()
        {
            todoList = new TodoList(repo, mapper, converter);
        }

        [Fact]
        public void should_return_task_id_on_add_task()
        {
            var args = new AddTaskArgs();
            repo.AddTask(args).Returns(1);
            var result = todoList.AddTask(args);
            result.Should().Be(1);
        }

        [Fact]
        public void should_get_task_from_repository()
        {
            var spec = new ListAllServiceSpecification();
            var expectedTasks = new List<ServiceTask>();
            repo.GetTasks(spec).Returns(expectedTasks);

            var resultList = todoList.GetTasks(spec);
            
            resultList.Should().BeEquivalentTo(expectedTasks);
        }

        [Fact]
        public void should_clear_task_date()
        {
            var task = new ServiceTask {Id = id};
            repo.Select(id).Returns(task);

            todoList.ClearDate(id);
            task.DueDate = default(DateTime);

            repo.Received().UpdateChanges(task);
        }

        [Fact]
        public void should_complete_task()
        {
            var task = new ServiceTask {Id = id};
            repo.Select(id).Returns(task);

            todoList.CompleteTask(id);
            task.IsCompleted = true;

            repo.Received().UpdateChanges(task);
        }

        [Fact]
        public void should_rename_task()
        {
            const string newName = "new name";
            var task = new ServiceTask { Id = id };
            repo.Select(id).Returns(task);

            todoList.RenameTask(id, newName);
            task.Name = newName;
            
            repo.Received().UpdateChanges(task);
        }

        [Fact]
        public void should_set_date_to_task()
        {
            var dueDate = DateTime.Today;
            var task = new ServiceTask {Id = id, DueDate = dueDate};
            repo.Select(id).Returns(task);

            todoList.SetTaskDate(id, dueDate);
            task.DueDate = dueDate;

            repo.Received().UpdateChanges(task);
        }
    }
}
