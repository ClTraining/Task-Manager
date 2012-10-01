using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerHost.DataBaseAccessLayer;
using Xunit;

namespace TaskManagerHost.TaskManager
{
    public class ToDoList : IToDoList
    {
        private readonly IRepository repository;

        public ToDoList(IRepository repository)
        {
            this.repository = repository;
        }

        public int AddTask(string name)
        {
            return repository.AddTask(name);
        }

        public ContractTask GetTaskById(int id)
        {
            var newTask = repository.GetTaskById(id);
            ContractTask result = null;

            if (newTask != null)
            {
                result = new ContractTask {Name = newTask.Name, Id = newTask.Id};
            }
            return result;
        }

        public List<ContractTask> GetAllTasks()
        {
            var newTasks = repository.GetAllTasks();
            return newTasks.Select(serviceTask => new ContractTask {Name = serviceTask.Name, Id = serviceTask.Id}).ToList();
        }


        public bool MarkCompleted(int id)
        {
            return repository.MarkCompleted(id);
        }

    }



    public class ToDoListTests
    {
        private readonly ContractTask incomingTask = new ContractTask();
        private readonly ServiceTask expectedTask = new ServiceTask();
        private readonly IRepository repository = Substitute.For<IRepository>();
        private readonly List<string> taskNames = new List<string> { "test task", "another task", "my task" };
        private readonly  IToDoList todolist;

        public  ToDoListTests()
        {
            todolist = new ToDoList(repository);
        }

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            repository.AddTask("new task").Returns(1);
            var newtask = todolist.AddTask("new task");
            newtask.Should().Be(1);
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found()
        {
            Action action = () => todolist.GetTaskById(1);
            action.ShouldThrow<Exception>().WithMessage("Task with id 1 was not found");
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var serviceTask = new ServiceTask();
            var contractTask = new ContractTask();
            repository.GetTaskById(1).Returns(serviceTask);
            var res = todolist.GetTaskById(1);
            res.Should().Be(contractTask);
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            Action action = () => todolist.MarkCompleted(1);
            action.ShouldThrow<Exception>().WithMessage("Task with id 10 was not found");
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            repository.MarkCompleted(1).Returns(true);
            var res = todolist.MarkCompleted(1);
            res.Should().Be(true);
        }
    }
}
