using EntitiesLibrary;
using NSubstitute;
using TaskManagerHost.DataBaseAccessLayer;
using Xunit;

namespace TaskManagerHost.TaskManager
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

        public ServiceTask AddTask(ITask task)
        {
            var newTask = _factory.Create();
            return _repository.AddTask(newTask);
        }
    }
    public class ToDoListTests
    {
        private readonly ITask _incomingTask = new ContractTask();
        private readonly ServiceTask _expectedTask = new ServiceTask();
        private readonly ITaskFactory _factory = Substitute.For<ITaskFactory>();
        private readonly IRepository _repository = Substitute.For<IRepository>();


        [Fact]
        public void todolist_asks_factory_for_new_task_and_saves_list()
        {
            var list = new ToDoList(_factory, _repository);
            _factory.Create().Returns(_expectedTask);

            list.AddTask(_incomingTask);
            _repository.Received().AddTask(_expectedTask);

        }
    }
}
