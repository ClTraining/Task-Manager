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

        public int AddTask(string name)
        {
            //var newTask = repository.AddTask(name);
            //var result = mapper.ConvertToContract(newTask);
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
        private readonly ITaskFactory factory = Substitute.For<ITaskFactory>();
        private readonly ITaskMapper mapper = new TaskMapper();
        private readonly IRepository memorepository = new MemoRepository();
        private readonly List<string> taskNames = new List<string> { "test task", "another task", "my task" };
        private readonly  IToDoList todolist;

        public  ToDoListTests()
        {
            todolist = new ToDoList(factory, memorepository, mapper);
        }

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var newtask = todolist.AddTask(taskNames[0]);
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
            var addedTasks = taskNames.Select(todolist.AddTask).ToList();
            var getedTasks = addedTasks.Select(contractTask => todolist.GetTaskById(contractTask)).ToList();
            foreach (var task in getedTasks)
            {
                addedTasks.ToArray()[getedTasks.IndexOf(task)].Should().Be(task.Id);
            }
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
            var addedTasks = taskNames.Select(todolist.AddTask).ToList();
            var editedTasks = addedTasks.Select(a => todolist.MarkCompleted(a)).ToList();
            var newTasks = todolist.GetAllTasks();
            foreach (var task in newTasks)
            {
                task.IsCompleted.Should().Be(true);
            }
        }
    }
}
