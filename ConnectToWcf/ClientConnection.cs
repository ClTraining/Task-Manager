using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.Arguments.AddTask;
using EntitiesLibrary.Arguments.CompleteTask;
using EntitiesLibrary.Arguments.RenameTask;
using EntitiesLibrary.Arguments.SetDate;
using Specifications.ClientSpecification;
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

        public void Complete(CompleteTaskArgs args)
        {
            UpdateDataOnServer(s => s.Complete(args));
        }

        public void RenameTask(RenameTaskArgs args)
        {
            UpdateDataOnServer(t => t.RenameTask(args));
        }

        public int AddTask(AddTaskArgs task)
        {
            return GetDataFromServer(t => t.AddTask(task));
        }

        public List<ContractTask> GetTasks(IClientSpecification data)
        {
            return GetDataFromServer(s => s.GetTasks(data));
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            UpdateDataOnServer(s => s.SetTaskDueDate(args));
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
            catch (FaultException<ExceptionDetail> e)
            {
                throw new TaskNotFoundException(e.Detail.Message);
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