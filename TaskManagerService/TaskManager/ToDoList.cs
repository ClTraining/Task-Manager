using EntitiesLibrary;
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

        public ServiceTask AddTask(ContractTask task)
        {
            var newTask = factory.Create();
            return repository.AddTask(newTask);
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
    }
}
