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
        private readonly ITaskFactory factory;
        private readonly IRepository repository;

        public ToDoList(ITaskFactory factory, IRepository repository)
        {
            this.factory = factory;
            this.repository = repository;
        }

        public ContractTask AddTask(ContractTask task)
        {
            var newTask = factory.Create();
            newTask.Name = task.Name;
            newTask.Id = task.Id;
            newTask = repository.AddTask(newTask);
            var result = new ContractTask {Name = newTask.Name, Id = newTask.Id};
            return result;
        }

        public ContractTask GetTaskById(int id)
        {
            var newTask = repository.GetTaskById(id);
            var result = new ContractTask { Name = newTask.Name, Id = newTask.Id };
            return result;
        }

        public List<ContractTask> GetAllTasks()
        {
            var newTasks = repository.GetAllTasks();
            return newTasks.Select(serviceTask => new ContractTask {Name = serviceTask.Name, Id = serviceTask.Id}).ToList();
        }

        public ContractTask EditTask(ContractTask task)
        {
            var newTask = factory.Create();
            newTask.Name = task.Name;
            newTask.Id = task.Id;
            newTask = repository.EditTask(newTask);
            var result = new ContractTask { Name = newTask.Name, Id = newTask.Id };
            return result;
        }

    }
    public class ToDoListTests
    {
        private readonly ContractTask incomingTask = new ContractTask();
        private readonly ServiceTask expectedTask = new ServiceTask();
        private readonly ITaskFactory factory = Substitute.For<ITaskFactory>();
        private readonly IRepository repository = Substitute.For<IRepository>();


        [Fact]
        public void todolist_asks_factory_for_new_task_and_saves_list()
        {
            //var list = new ToDoList(factory, repository);
            //factory.Create().Returns(expectedTask);

            //list.AddTask(incomingTask);
            //repository.Received().AddTask(expectedTask);

        }

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var memorepository = new MemoRepository();
            var fact = new TaskFactory();
            var todolist = new ToDoList(fact, memorepository);
            var task = new ContractTask { Id = 0 };
            var newtask = todolist.AddTask(task);
            newtask.Id.Should().Be(1);
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found()
        {
            var memorepository = new MemoRepository();
            var fact = new TaskFactory();
            var todolist = new ToDoList(fact, memorepository);
            Action action = () => todolist.GetTaskById(1);
            action.ShouldThrow<Exception>().WithMessage("Task with id 1 was not found");
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var memorepository = new MemoRepository();
            var fact = new TaskFactory();
            var todolist = new ToDoList(fact, memorepository);
            todolist.AddTask(new ContractTask{ Id = 10, Name = "test task" });
            todolist.AddTask(new ContractTask { Id = 0, Name = "another task" });
            todolist.AddTask(new ContractTask { Id = 0, Name = "my task" });
            var task1 = todolist.GetTaskById(1);
            var task2 = todolist.GetTaskById(2);
            var task3 = todolist.GetTaskById(3);
            task1.Name.Should().Be("test task");
            task2.Name.Should().Be("another task");
            task3.Name.Should().Be("my task");
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            var memorepository = new MemoRepository();
            var fact = new TaskFactory();
            var todolist = new ToDoList(fact, memorepository);
            Action action = () => todolist.EditTask(new ContractTask { Id = 10, Name = "test task" });
            action.ShouldThrow<Exception>().WithMessage("Task with id 10 was not found");
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            var memorepository = new MemoRepository();
            var fact = new TaskFactory();
            var todolist = new ToDoList(fact, memorepository);
            todolist.AddTask(new ContractTask { Id = 10, Name = "test task" });
            todolist.AddTask(new ContractTask { Id = 0, Name = "another task" });
            todolist.AddTask(new ContractTask { Id = 0, Name = "my task" });
            todolist.EditTask(new ContractTask { Id = 1, Name = "new test task" });
            todolist.EditTask(new ContractTask { Id = 2, Name = "new another task" });
            todolist.EditTask(new ContractTask { Id = 3, Name = "new my task" });
            var task1 = todolist.GetTaskById(1);
            var task2 = todolist.GetTaskById(2);
            var task3 = todolist.GetTaskById(3);
            task1.Name.Should().Be("new test task");
            task2.Name.Should().Be("new another task");
            task3.Name.Should().Be("new my task");
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var memoRepository = new MemoRepository();
            var taskList = memoRepository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>());
            var task1 = new ServiceTask { Id = 10, Name = "test task" };
            var task2 = new ServiceTask { Id = 10, Name = "test task1" };
            task1 = memoRepository.AddTask(task1);
            task2 = memoRepository.AddTask(task2);
            taskList = memoRepository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask> { task1, task2 });
        }
    }
}
