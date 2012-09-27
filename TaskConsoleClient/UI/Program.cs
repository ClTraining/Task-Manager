using System;
using Ninject;
using Ninject.Modules;
using TaskConsoleClient.Manager;

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            var module = new TaskManagerModule();
            var kernel = new StandardKernel(module);
            var consoleHelper = kernel.Get<IConsoleHelper>();
            string s;
            while ((s = Console.ReadLine()) != null)
            {
                consoleHelper.Parse(s);
            }
        }
    }
    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConsoleHelper>().To<ConsoleHelper>();
            Bind<ICommandManager>().To<CommandManager>();
        }
    }
}
