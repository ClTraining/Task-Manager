using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerApp.DataBaseAccessLayer;
using TaskManagerHost.TaskManager;
using Xunit;

namespace TaskManagerApp.TaskManager
{
    public class ToDoList : IToDoList
    {
        private readonly ITaskFactory _factory;
        private readonly IRepository _repository;

        public ToDoList(ITaskFactory factory, IRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public ITask AddTask(ITask task)
        {
            var newTask = _factory.Create();
            return _repository.AddTask(newTask);
        }
    }
    public class ToDoListTests
    {
        private readonly ITask incomingTask = new ContractTask();
        private readonly ServiceTask expectedTask = new ServiceTask();
        private readonly ITaskFactory factory = Substitute.For<ITaskFactory>();
        private IRepository repository = Substitute.For<IRepository>();


        [Fact]
        public void todolist_asks_factory_for_new_task_and_saves_list()
        {
            var list = new ToDoList(factory, repository);
            factory.Create().Returns(expectedTask);

            list.AddTask(incomingTask);
            repository.Received().AddTask(expectedTask);

        }
    }
}
