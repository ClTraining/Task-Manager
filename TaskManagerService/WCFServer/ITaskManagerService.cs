using System.ServiceModel;
using EntitiesLibrary;

namespace TaskManagerService.WCFServer
{
    [ServiceContract]
    public interface ITaskManagerService
    {
        [OperationContract]
        ITask AddTask(ITask task);
    }
}
