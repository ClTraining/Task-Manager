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
            try
            {
                return GetDataFromServer(t => t.AddTask(task));
            }
            catch (FaultException<ExceptionDetail>)
            {
                throw new TaskNotFoundException("Wrong operation!");
            }
        }

        public List<ContractTask> GetTaskById(int id)
        {
            try
            {
                return GetDataFromServer(s => new List<ContractTask> { s.GetTaskById(id) });
            }
            catch (FaultException<ExceptionDetail>)
            {
                throw new TaskNotFoundException(id);
            }
        }

        public List<ContractTask> GetAllTasks()
        {
            try
            {
                return GetDataFromServer(s => s.GetAllTasks());
            }
            catch (FaultException<ExceptionDetail>)
            {
                throw new TaskNotFoundException("Wrong operation!");
            }
        }

        public void Complete(int id)
        {
            try
            {
                UpdateDataOnServer(s => s.Complete(id));
            }
            catch (FaultException<ExceptionDetail>)
            {
                throw new TaskNotFoundException(id);
            }
        }

        private void UpdateDataOnServer(Action<ITaskManagerService> action)
        {
            GetDataFromServer<object>(s =>
            {
                action(s);
                return null;
            });
        }

        private T GetDataFromServer<T>(Func<ITaskManagerService, T> func)
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