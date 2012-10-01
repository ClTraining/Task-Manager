using System;
using System.ServiceModel;
using Ninject;
using Ninject.Modules;
using TaskManagerHost.DataBaseAccessLayer;
using TaskManagerHost.TaskManager;


namespace TaskManagerHost.WCFServer
{
    static class TaskManagerApplication
    {
        private static void Main()
        {
            IKernel kernel = new StandardKernel(new TaskManagerModule());
            using (var serviceHost = new ServiceHost(kernel.Get<IToDoList>()))
            {
                serviceHost.Open();
                Console.WriteLine("Host started");
                Console.WriteLine("Press Enter to terminate the host...");
                Console.ReadLine();
            }
        }
    }

    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository>().To<MemoRepository>();
            Bind<ITaskFactory>().To<TaskFactory>();
            Bind<IToDoList>().To<ToDoList>();
            Bind<ITaskMapper>().To<TaskMapper>();
        }
    }

}