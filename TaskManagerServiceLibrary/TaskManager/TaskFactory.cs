using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerServiceLibrary.TaskManager
{
    public class TaskFactory : ITaskFactory
    {
        #region ITaskFactory Members

        public ServiceTask Create()
        {
            return new ServiceTask();
        }

        #endregion
    }

    public class TaskFactoryTests
    {
        [Fact]
        public void should_create_new_task()
        {
            ITaskFactory taskFactory = new TaskFactory();
            ServiceTask result = taskFactory.Create();
            result.Should().BeOfType<ServiceTask>();
        }
    }
}