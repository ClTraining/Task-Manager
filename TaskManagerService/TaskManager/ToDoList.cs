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
        private ITaskMapper mapper;

        public ToDoList(IRepository repository, ITaskMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public int AddTask(string name)
        {
            return repository.AddTask(name);
        }

        public ContractTask GetTaskById(int id)
        {
            var newTask = repository.GetTaskById(id);
            return mapper.ConvertToContract(newTask);
        }

        public List<ContractTask> GetAllTasks()
        {
            var newTasks = repository.GetAllTasks();
            return newTasks.Select(mapper.ConvertToContract).ToList();
        }


        public bool MarkCompleted(int id)
        {
            return repository.MarkCompleted(id);
        }
    }

    public class ToDoListTests
    {
        private readonly IRepository repository = Substitute.For<IRepository>();
        private readonly ITaskMapper mapper = Substitute.For<ITaskMapper>();
        private readonly  IToDoList todolist;

        public  ToDoListTests()
        {
            todolist = new ToDoList(repository, mapper);
        }

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            repository.AddTask("new task").Returns(1);
            var newtask = todolist.AddTask("new task");
            newtask.Should().Be(1);
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var serviceTask = new ServiceTask();
            var contractTask = new ContractTask();
            repository.GetTaskById(1).Returns(serviceTask);
            mapper.ConvertToContract(serviceTask).Returns(contractTask);

            var res = todolist.GetTaskById(1);
            res.Should().Be(contractTask);
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var serviceTasks = new List<ServiceTask> { new ServiceTask { Id = 1, Name = "some" } };
            repository.GetAllTasks().Returns(serviceTasks);

            var contractTasks = serviceTasks.Select(task => mapper.ConvertToContract(task)).ToList();

            var res = todolist.GetAllTasks();
            res.Should().BeEquivalentTo(contractTasks);
        }

        [Fact]
        public void should_mark_task_as_completed_by_id()
        {
            repository.MarkCompleted(1).Returns(true);
            var res = todolist.MarkCompleted(1);
            res.Should().Be(true);
        }
    }
}
