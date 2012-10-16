using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter
{
    public interface ITaskFormatter
    {
        string Show(List<ContractTask> tasks);
    }
}