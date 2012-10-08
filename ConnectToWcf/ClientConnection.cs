using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using TaskManagerServiceLibrary;

namespace ConnectToWcf
{
    public class ClientConnection : IClientConnection
    {
        public int AddTask(string task)
        {
            var client = new ChannelFactory<ITaskManagerService>("tcpEndPoint");
            client.Open();
            try
            {
                return client.CreateChannel().AddTask(task);
            }
            finally
            {
                CloseClient(client);
            }
        }


        public ContractTask GetTaskById(int id)
        {
            var client = new ChannelFactory<ITaskManagerService>("tcpEndPoint");
            client.Open();
            try
            {
                return client.CreateChannel().GetTaskById(id);
            }
            finally
            {
                CloseClient(client);
            }
        }

        public List<ContractTask> GetAllTasks()
        {
            var client = new ChannelFactory<ITaskManagerService>("tcpEndPoint");
            client.Open();
            try
            {
                return client.CreateChannel().GetAllTasks();
            }
            finally
            {
                CloseClient(client);
            }
        }

        public void MarkCompleted(int id)
        {
            var client = new ChannelFactory<ITaskManagerService>("tcpEndPoint");
            client.Open();
            try
            {
                client.CreateChannel().MarkCompleted(id);
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