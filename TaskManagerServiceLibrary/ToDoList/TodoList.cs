using System;
using System.Collections.Generic;
using System.Linq;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.Repositories.TaskExtractor;
using TaskManagerServiceLibrary.TaskMapper;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary.ToDoList
{
    public class ToDoList : ITodoList
    {
        private readonly IRepository repo;
        private readonly ITaskMapper mapper;
        private readonly ITaskExtractor taskExtractor;

        public ToDoList(IRepository repo, ITaskMapper mapper, ITaskExtractor taskExtractor)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.taskExtractor = taskExtractor;
        }

        public int AddTask(AddTaskArgs args)
        {
            var task = mapper.ConvertArgsToTask(args);
            var id = repo.AddTask(task);
            return id;
        }

        public List<ClientTask> GetTasks(IServiceSpecification serviceSpecification)
        {
            return repo.GetTasks(serviceSpecification)
                .Select(mapper.ConvertToClient)
                .ToList();
        }

        public void ClearDate(int id)
        {
            var task = SelectTaskById(id);
            if (task.IsCompleted) throw new CouldNotSetDateException("Could not clear due date for completed task.");
            task.DueDate = default(DateTime);
            repo.UpdateChanges(task);
        }

        public void CompleteTask(int id)
        {
            var task = SelectTaskById(id);
            task.IsCompleted = true;
            repo.UpdateChanges(task);
        }

        public void RenameTask(int id, string newName)
        {
            var task = SelectTaskById(id);
            task.Name = newName;
            repo.UpdateChanges(task);
        }

        public void SetTaskDate(int id, DateTime dueDate)
        {
            var task = SelectTaskById(id);
            if (task.IsCompleted) throw new CouldNotSetDateException("Could not set due date to completed task.");
            task.DueDate = dueDate;
            repo.UpdateChanges(task);
        }

        public ServiceTask SelectTaskById(int id)
        {
            return taskExtractor.SelectTaskById(id);
        }
    }

    public class TodoListTests
    {
        private readonly IRepository repo = Substitute.For<IRepository>();
        private readonly ITaskMapper mapper = Substitute.For<ITaskMapper>();
        private readonly ToDoList todoList;
        private readonly ITaskExtractor extractor = Substitute.For<ITaskExtractor>();

        public TodoListTests()
        {
            todoList = new ToDoList(repo, mapper, extractor);
        }

        [Fact]
        public void should_return_task_id_on_add_task()
        {
            var args = new ServiceTask();
            var addTaskArgs = new AddTaskArgs();
            mapper.ConvertArgsToTask(addTaskArgs).Returns(args);

            repo.AddTask(args).Returns(1);
            var result = todoList.AddTask(addTaskArgs);
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
            const int id = 5;
            var task = new ServiceTask { DueDate = DateTime.Today };
            extractor.SelectTaskById(id).Returns(task);

            todoList.ClearDate(5);
            repo.Received().UpdateChanges(task);
        }

        [Fact]
        public void should_throw_exception_on_clear_date_for_completed_task()
        {
            const int id = 5;
            var task = new ServiceTask { Id = id, IsCompleted = true };
            extractor.SelectTaskById(id).Returns(task);

            Action act = () => todoList.ClearDate(id);

            act.ShouldThrow<CouldNotSetDateException>().WithMessage("Could not clear due date for completed task.");
        }

        [Fact]
        public void should_complete_task()
        {
            const int id = 5;
            var task = new ServiceTask { DueDate = DateTime.Today, IsCompleted = false };
            extractor.SelectTaskById(id).Returns(task);

            todoList.CompleteTask(5);
            repo.Received().UpdateChanges(task);
        }

        [Fact]
        public void should_rename_task()
        {
            const int id = 5;
            var task = new ServiceTask { DueDate = DateTime.Today, IsCompleted = false };
            extractor.SelectTaskById(id).Returns(task);

            todoList.RenameTask(5, "sasha");
            repo.Received().UpdateChanges(task);
        }

        [Fact]
        public void should_set_date_to_task()
        {
            const int id = 5;
            var task = new ServiceTask { DueDate = DateTime.Today, IsCompleted = false };
            extractor.SelectTaskById(id).Returns(task);

            todoList.SetTaskDate(5, DateTime.Today);
            repo.Received().UpdateChanges(task);
        }

        [Fact]
        public void should_throw_exception_on_set_date_to_completed_task()
        {
            const int id = 5;
            var dueDate = DateTime.Today;
            var task = new ServiceTask { Id = id, DueDate = dueDate, IsCompleted = true };
            extractor.SelectTaskById(id).Returns(task);

            Action action = () => todoList.SetTaskDate(id, dueDate);

            action.ShouldThrow<CouldNotSetDateException>().WithMessage("Could not set due date to completed task.");
        }
    }
}
