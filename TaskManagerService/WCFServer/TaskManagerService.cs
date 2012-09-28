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
        private readonly IToDoList taskList;

        public TaskManagerService()
        {
            taskList = new StandardKernel(new TaskManagerModule()).Get<ToDoList>();

            Console.WriteLine("new request added");
        }

        public ContractTask AddTask(ContractTask task)
        {
            return taskList.AddTask(task);
        }

        public ContractTask GetTaskById(int id)
        {
            return taskList.GetTaskById(id);
        }

        public List<ContractTask> GetAllTasks()
        {
            return taskList.GetAllTasks();
        }

        public ContractTask Edit(ContractTask task)
        {
            return null;
        }
    }

    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository>().To<XmlRepository>().InSingletonScope();
            Bind<ITaskFactory>().To<TaskFactory>();
            Bind<IToDoList>().To<ToDoList>().InSingletonScope();
            Bind<ITaskMapper>().To<TaskMapper>();
        }
    }

    public class TaskManagerServiceTests
    {
        [Fact]
        public void should_send_and_return_task()
        {
        }
    }
}