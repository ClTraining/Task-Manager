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
            catch (FaultException<ExceptionDetail> e)
            {
                Console.WriteLine(e.Detail.Message);
                throw new NullReferenceException();
            }
            finally
            {
                CloseClient(client);
            }
        }


        public List<ContractTask> GetTaskById(int id)
        {
            var client = new ChannelFactory<ITaskManagerService>("tcpEndPoint");
            client.Open();
            try
            {
                return new List<ContractTask> { client.CreateChannel().GetTaskById(id) };
            }
            catch (FaultException<ExceptionDetail> e)
            {
                Console.WriteLine(e.Detail.Message);
                throw new NullReferenceException();
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
            catch (FaultException<ExceptionDetail> e)
            {
                Console.WriteLine(e.Detail.Message);
                throw new NullReferenceException();
            }
            finally
            {
                CloseClient(client);
            }
        }

        public void Complete(int id)
        {
            var client = new ChannelFactory<ITaskManagerService>("tcpEndPoint");
            client.Open();
            try
            {
                client.CreateChannel().Complete(id);
            }
            catch (FaultException<ExceptionDetail> e)
            {
                Console.WriteLine(e.Detail.Message);
                throw new NullReferenceException();
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