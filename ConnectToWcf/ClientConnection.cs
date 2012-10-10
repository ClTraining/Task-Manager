using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using TaskManagerServiceLibrary;

namespace ConnectToWcf
{
    public class ClientConnection : IClientConnection
    {
        private readonly string serviceAddress;
        private readonly NetTcpBinding binding;
        public ClientConnection(string address)
        {
            serviceAddress = address;
            binding = new NetTcpBinding();
        }

        public int AddTask(string task)
        {
            return GetSomethingFromServer(t => t.AddTask(task));
        }

        public List<ContractTask> GetTaskById(int id)
        {
            return GetSomethingFromServer(s => new List<ContractTask> {s.GetTaskById(id)});
        }

        public List<ContractTask> GetAllTasks()
        {
            return GetSomethingFromServer(s => s.GetAllTasks());
        }

        public void Complete(int id)
        {
            DoSomethingOnServer(s => s.Complete(id));
        }

        private void DoSomethingOnServer(Action<ITaskManagerService> action)
        {
            GetSomethingFromServer<object>(s =>
            {
                action(s);
                return null;
            });
        }

        private T GetSomethingFromServer<T>(Func<ITaskManagerService, T> func)
        {
            var client = new ChannelFactory<ITaskManagerService>(binding, serviceAddress);
            client.Open();
            try
            {
                return func(client.CreateChannel());
            }
            finally
            {
                CloseClient(client);
            }
        }

        private static void CloseClient(ChannelFactory<ITaskManagerService> client)
        {
            try
            {
                client.Close();
            }
            catch (TimeoutException)
            {
                client.Abort();
            }
            catch (CommunicationException)
            {
                client.Abort();
            }
            catch (Exception)
            {
                client.Abort();
                throw;
            }
        }
    }
}