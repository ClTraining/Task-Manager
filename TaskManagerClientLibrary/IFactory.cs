using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesLibrary.CommandArguments;
using TaskManagerClientLibrary.ConcreteCommands.TaskFormatter;

namespace TaskManagerClientLibrary
{
    public interface IFactory
    {
        ITaskFormatter GetFormatter(IListCommandArguments specification);
    }
}
