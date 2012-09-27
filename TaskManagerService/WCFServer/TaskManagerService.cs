using System;
using System.Collections.Generic;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Ninject;
using Ninject.Modules;
using TaskManagerHost.DataBaseAccessLayer;
using TaskManagerHost.TaskManager;
using Xunit;

namespace TaskManagerHost.WCFServer
{
    public class TaskManagerService : ITaskManagerService
    {
        private readonly TaskManagerModule module;
        private readonly IKernel kernel;
        private readonly IToDoList taskList;
        
        public TaskManagerService()
        {
            module = new TaskManagerModule();
            kernel = new StandardKernel(module);
            taskList = kernel.Get<ToDoList>();

            Console.WriteLine("added new task");
        }

        public ContractTask AddTask(ContractTask task)
        {
            var sTask = taskList.AddTask(task);
            return new ContractTask {Id = sTask.Id, Name = sTask.Name};
        }

        public List<ContractTask> ViewAllTasks()
        {
            return null;
        }

        public ContractTask ViewTaskById(int id)
        {
            return null;
        }

        public ContractTask IsCompleted(ContractTask task)
        {
            return null;
        }
    }

    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository>().To<MemoRepository>();
            Bind<ITaskFactory>().To<TaskFactory>();
            Bind<IToDoList>().To<ToDoList>();
        }
    }

    public class TaskManagerServiceTests
    {
        private readonly ServiceTask incomingTask = new ServiceTask();
        private readonly ContractTask outgoingTask = new ContractTask();
        private readonly IToDoList list = Substitute.For<IToDoList>();
        private readonly ITaskManagerService manager;

        public TaskManagerServiceTests()
        {
            manager = new TaskManagerService();
        }

        [Fact]
        public void should_send_and_return_task()
        {
        }
    }
}