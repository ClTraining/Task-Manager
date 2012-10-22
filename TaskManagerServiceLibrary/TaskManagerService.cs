using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Specifications.ClientSpecification;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;
using System.Linq;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly IRepository repository;
        private readonly IToDoList taskList;
        private readonly List<IServiceSpecification> list;

        public TaskManagerService(IRepository repository, List<IServiceSpecification> list, IToDoList taskList)
        {
            this.repository = repository;
            this.list = list;
            this.taskList = taskList;
        }

        public int AddTask(string task)
        {
            return taskList.AddTask(task);
        }

        public List<ContractTask> GetTasks(IClientSpecification specification)
        {
            var res = list.First(x => x.GetType().Name.Contains(specification.GetType().Name));
            res.Data = specification.Data;
            return repository.GetTasks(res);
        }
    }

    public class TaskManagerServiceTests
    {
        [Fact]
        public void should_get_tasks_from_repository()
        {
            List<IServiceSpecification> specs = new StandardKernel(new MyModule()).GetAll<IServiceSpecification>().ToList();

            object id = 3;
            var cSpec = new ListSingle(4);

            var repo = new MemoRepository();
            var todolist = new ToDoList(repo);

            var service = new TaskManagerService(repo, specs, todolist);

            var tasks = new[] { "task1", "task2", "task3", "task4", "task5", "task6", "task7", "task8", "task9" };

            tasks.ToList().ForEach(x => service.AddTask(x));

            var result = service.GetTasks(cSpec);

            foreach (var task in result)
            {
                Console.Out.WriteLine(task.Id + " " + task.Name);
            }
        }

        public class MyModule : NinjectModule
        {
            public override void Load()
            {
                this.Bind(a => a.FromAssemblyContaining<IServiceSpecification>()
                                   .SelectAllClasses()
                                   .InNamespaceOf<IServiceSpecification>()
                                   .BindAllInterfaces()
                    );
            }
        }
    }
}