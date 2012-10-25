using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerClientLibrary.ConcreteCommands.TaskFormatter
{
    public interface ITaskFormatter
    {
        string ToFormatString(List<ClientPackage> tasks);
    }
}