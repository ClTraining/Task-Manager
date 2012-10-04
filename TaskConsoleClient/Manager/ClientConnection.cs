using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using TaskManagerService.WCFService;

namespace TaskConsoleClient.Manager
{
    public class ClientConnection : IClientConnection
    {
        private readonly ChannelFactory<ITaskManagerService> client;
            
            public ClientConnection()
            {
                client = new ChannelFactory<ITaskManagerService>("tcpEndPoint");
                client.Open();
            }

        public int AddTask(string task)
        {
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