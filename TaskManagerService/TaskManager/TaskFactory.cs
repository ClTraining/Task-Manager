using System;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerService.DataBaseAccessLayer;
using Xunit;

namespace TaskManagerService.TaskManager
{
    public class TaskFactory : ITaskFactory
    {
        private readonly IDataBaseManager manager;

        public TaskFactory(IDataBaseManager manager)
        {
            this.manager = manager;
        }

        public ServiceTask Create()
        {
            return new ServiceTask { Id = manager.GetId(), Name = string.Empty };
        }
    }

    public class TaskFactoryTests
    {
        [Fact]
        public void factory_should_create_empty_service_task()
        {
            var manager = Substitute.For<IDataBaseManager>();
            var expTask = new ServiceTask { Id = 0, Name = string.Empty };
            manager.GetId().Returns(0);
            var factory = new TaskFactory(manager);
            factory.Create().Returns(expTask);
            var task = factory.Create();
            task.Should().Be(expTask);
        }
    }
}