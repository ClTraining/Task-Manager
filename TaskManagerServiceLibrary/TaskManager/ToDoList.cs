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
        private readonly ITaskMapper mapper;

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
            return mapper.ConvertToContract(repository.GetTaskById(id));
        }

        public List<ContractTask> GetAllTasks()
        {
            return repository.GetAllTasks()
                .Select(mapper.ConvertToContract)
                .ToList();
        }

        public void Complete(int id)
        {
            repository.Complete(id);
        }
    }


    public class ToDoListTests
    {
        private readonly IRepository repository = Substitute.For<IRepository>();
        private readonly ITaskMapper mapper = Substitute.For<ITaskMapper>();
        private readonly  IToDoList todolist;

        public ToDoListTests()
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
            var serviceTask = new ServiceTask {Id = 1, Name = "some"};
            var serviceTasks = new List<ServiceTask> {serviceTask};
            var contractTask = new ContractTask {Id = 1, Name = "some"};
            var contractTasks = new List<ContractTask> {contractTask};

            repository.GetAllTasks().Returns(serviceTasks);
            mapper.ConvertToContract(serviceTask).Returns(contractTask);

            var res = todolist.GetAllTasks();
            res.Should().BeEquivalentTo(contractTasks);
        }

        [Fact]
        public void should_mark_task_as_completed_by_id()
        {
            todolist.Complete(1);
            repository.Received().Complete(1);
        }
    }
}