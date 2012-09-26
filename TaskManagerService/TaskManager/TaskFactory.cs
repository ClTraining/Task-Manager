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
        public ServiceTask Create()
        {
            return new ServiceTask();
        }
    }

    public class TaskFactoryTests
    {
        [Fact]
        public void should_create_new_task()
        {
            ITaskFactory taskFactory= new TaskFactory();
            var result = taskFactory.Create();
            result.Should().BeOfType<ServiceTask>();
        }

    }
}