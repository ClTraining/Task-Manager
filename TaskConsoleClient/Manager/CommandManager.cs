using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using NSubstitute;
using TaskManagerHost.WCFServer;
using Xunit;

namespace TaskConsoleClient.Manager
{
    class CommandManager: ICommandManager
    {
        private readonly IConnection conn;

        public CommandManager(IConnection conn)
        {
            this.conn = conn;
        }

        public ContractTask AddTask(ContractTask task)
        {
            return conn.GetClient().AddTask(task); ;
        }

        public ContractTask GetTaskById(int id)
        {
            return conn.GetClient().GetTaskById(id);
        }

        public ContractTask Edit(ContractTask task)
        {
            return conn.GetClient().Edit(task);
        }

        public List<ContractTask> GetAllTasks()
        {
            return conn.GetClient().GetAllTasks();
        }
    }

    public interface IConnection
    {
        ITaskManagerService GetClient();
    }

    public class NetTcpConnection : IConnection
    {
        private ChannelFactory<ITaskManagerService> factory;
        private ITaskManagerService client;

        public ITaskManagerService GetClient()
        {
            factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444");

            client = factory.CreateChannel();
            return client;
        }
    }

    public class CommandManagerTests
    {
        private readonly ITaskManagerService service = Substitute.For<ITaskManagerService>();
        readonly ContractTask task = new ContractTask();

        [Fact]
        public void should_send_add_task_to_service()
        {
        }
    }
}