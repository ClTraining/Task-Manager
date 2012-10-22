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

        public int AddTask(string name)
        {
            return repository.AddTask(name);
        }

        public void Complete(int id)
        {
            repository.Complete(id);
        }

        public void RenameTask(RenameTaskArgs args)
        {
            repository.RenameTask(args);
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
            repository.AddTask("new task").Returns(1);
            int newtask = todolist.AddTask("new task");
            newtask.Should().Be(1);
        }

        [Fact]
        public void should_mark_task_as_completed_by_id()
        {
            todolist.Complete(1);
            repository.Received().Complete(1);
        }

        [Fact]
        public void should_send_rename_task_to_repository()
        {
            var args = new RenameTaskArgs {Id = 1, Name = "task name"};
            todolist.RenameTask(args);
            repository.Received().RenameTask(args);
        }
    }
}