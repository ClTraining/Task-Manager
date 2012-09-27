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
            var list = new ToDoList(factory, repository);
            factory.Create().Returns(expectedTask);

            list.AddTask(incomingTask);
            repository.Received().AddTask(expectedTask);
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
    }
}
