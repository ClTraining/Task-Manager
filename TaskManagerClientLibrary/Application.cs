﻿using System;
using System.Configuration;
using ConnectToWcf;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using TaskManagerClientLibrary.ConcreteHandlers;
using TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter;

namespace TaskManagerClientLibrary
{
    public class Application
    {
        public void Run()
        {
            Console.Title = "Task Manager Client";

            var module = new TaskManagerModule();

            var kernel = new StandardKernel(module);

            for (string s; ((s = Console.ReadLine()) != null);)

                kernel.Get<LineParser>().ExecuteCommand(s);
        }
    }

    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromAssemblyContaining<ICommand>().SelectAllClasses()
                               .InNamespaceOf<ICommand>()
                               .BindAllInterfaces().Configure(b => b.WithConstructorArgument("textWriter", Console.Out))
                               );
            Bind<ArgumentConverter<object>>().ToSelf();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var address = config.AppSettings.Settings["connectionAddress"].Value;
            Bind<IClientConnection>().To<ClientConnection>().WithConstructorArgument("address",address);

            var factoryTaskFormatterMethod = new Func<string, ITaskFormatter>
                (input =>
                     {
                         if (string.IsNullOrEmpty(input))
                         {
                             return Kernel.Get<TableFormatter>();
                         }
                         return Kernel.Get<ListFormatter>();
                     });

            Bind<Func<string, ITaskFormatter>>().ToConstant(factoryTaskFormatterMethod);
        }
    }
}
