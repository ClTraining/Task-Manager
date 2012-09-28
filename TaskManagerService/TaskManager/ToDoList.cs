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
        private readonly ITaskMapper mapper;

        public ToDoList(ITaskFactory factory, IRepository repository, ITaskMapper mapper)
        {
            this.factory = factory;
            this.repository = repository;
            this.mapper = mapper;
        }

        public ContractTask AddTask(ContractTask task)
        {
            var newTask = mapper.ConvertToService(task);
            newTask = repository.AddTask(newTask);
            var result = mapper.ConvertToContract(newTask);
            return result;
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

        public ContractTask EditTask(ContractTask task)
        {
            var newTask = mapper.ConvertToService(task);
            newTask = repository.EditTask(newTask);
            var result = mapper.ConvertToContract(newTask);
            return result;
        }

    }



    public class ToDoListTests
    {
        private readonly ContractTask incomingTask = new ContractTask();
        private readonly ServiceTask expectedTask = new ServiceTask();
        private readonly ITaskFactory factory = Substitute.For<ITaskFactory>();
        private readonly ITaskMapper mapper = new TaskMapper();
        private readonly IRepository memorepository = new MemoRepository();

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var todolist = new ToDoList(factory, memorepository, mapper);
            var task = new ContractTask { Id = 0 };
            var newtask = todolist.AddTask(task);
            newtask.Id.Should().Be(1);
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found()
        {
            var todolist = new ToDoList(factory, memorepository, mapper);
            Action action = () => todolist.GetTaskById(1);
            action.ShouldThrow<Exception>().WithMessage("Task with id 1 was not found");
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var todolist = new ToDoList(factory, memorepository, mapper);
            var tasks = new List<ContractTask> { new ContractTask { Id = 10, Name = "test task" } ,
            new ContractTask { Id = 0, Name = "another task" },
            new ContractTask { Id = 0, Name = "my task" }
            };
            var addedTasks = tasks.Select(todolist.AddTask).ToList();
            var getedTasks = addedTasks.Select(contractTask => todolist.GetTaskById(contractTask.Id)).ToList();
            foreach (var task in getedTasks)
            {
                addedTasks.ToArray()[getedTasks.IndexOf(task)].Name.Should().Be(task.Name);
            }
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            var todolist = new ToDoList(factory, memorepository, mapper);
            Action action = () => todolist.EditTask(new ContractTask { Id = 10, Name = "test task" });
            action.ShouldThrow<Exception>().WithMessage("Task with id 10 was not found");
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            var todolist = new ToDoList(factory, memorepository, mapper);
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
            //var taskList = memoRepository.GetAllTasks();
            //taskList.Should().BeEquivalentTo(new List<ServiceTask>());
            //var task1 = new ServiceTask { Id = 10, Name = "test task" };
            //var task2 = new ServiceTask { Id = 10, Name = "test task1" };
            //task1 = memoRepository.AddTask(task1);
            //task2 = memoRepository.AddTask(task2);
            //taskList = memoRepository.GetAllTasks();
            //taskList.Should().BeEquivalentTo(new List<ServiceTask> { task1, task2 });
        }
    }
}
