using System;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerApp.DataBaseAccessLayer;
using Xunit;

namespace TaskManagerApp.TaskManager
{
    public class TaskFactory : ITaskFactory
    {
        public ServiceTask Create(ITask task)
        {
            return null;
        }
    }

    public class TaskFactoryTests
    {
    }
}