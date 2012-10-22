using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;

namespace TaskManagerServiceLibrary.TaskManager
{
    public class ToDoList : IToDoList
    {
        private readonly IRepository repository;

        public ToDoList(IRepository repository)
        {
            this.repository = repository;
        }

        #region IToDoList Members

        public int AddTask(AddTaskArgs name)
        {
            return repository.AddTask(name);
        }

        public void MarkTaskAsCompleted(CompleteTaskArgs id)
        {
            repository.MarkTaskAsCompleted(id);
        }

        public void RenameTask(RenameTaskArgs args)
        {
            repository.RenameTask(args);
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            repository.SetTaskDueDate(args);
        }

        #endregion
    }


    public class ToDoListTests
    {
        private readonly IRepository repository = Substitute.For<IRepository>();
        private readonly IToDoList todolist;

        public ToDoListTests()
        {
            todolist = new ToDoList(repository);
        }

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var addTaskArgs = new AddTaskArgs {Name = "new task"};
            repository.AddTask(addTaskArgs).Returns(1);
            int newtask = todolist.AddTask(addTaskArgs);
            newtask.Should().Be(1);
        }

        [Fact]
        public void should_mark_task_as_completed_by_id()
        {
            var completeTaskArgs = new CompleteTaskArgs {Id = 1};
            todolist.MarkTaskAsCompleted(completeTaskArgs);
            repository.Received().MarkTaskAsCompleted(completeTaskArgs);
        }

        [Fact]
        public void should_send_rename_task_to_repository()
        {
            var args = new RenameTaskArgs {Id = 1, Name = "task name"};
            todolist.RenameTask(args);
            repository.Received().RenameTask(args);
        }

        [Fact]
        public void should_send_set_date_for_task_to_repository()
        {
            var args = new SetDateArgs { Id = 1, DueDate = DateTime.Now };
            todolist.SetTaskDueDate(args);
            repository.Received().SetTaskDueDate(args);
        }
    }
}