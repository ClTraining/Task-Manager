#region Using

using EntitiesLibrary;
using FluentAssertions;
using Xunit;

#endregion


namespace TaskManagerHost.TaskManager
{
    public class TaskMapper:ITaskMapper
    {
        public ServiceTask ConvertToService(ContractTask task)
        {
            var newTask = new ServiceTask {Id = task.Id, Name = task.Name, IsCompleted = task.IsCompleted};
            return newTask;
        }

        public ContractTask ConvertToContract(ServiceTask task)
        {
            return task != null ? new ContractTask { Id = task.Id, Name = task.Name, IsCompleted = task.IsCompleted } : null;
        }
    }

    public class TaskMapperTests
    {
        private readonly ServiceTask serviceTask = new ServiceTask {Id = 10, Name = "service", IsCompleted = true};
        private readonly ContractTask contractTask = new ContractTask {Id = 11, Name = "contract", IsCompleted = false};
        private readonly TaskMapper taskMapper = new TaskMapper();

        [Fact]
        public void should_convert_service_to_contract()
        {
            var result = taskMapper.ConvertToContract(serviceTask);
            result.Should().BeOfType<ContractTask>();
            result.Name.Should().Be(serviceTask.Name);
            result.Id.Should().Be(serviceTask.Id);
            result.IsCompleted.Should().Be(serviceTask.IsCompleted);
        }

        [Fact]
        public void should_convert_contract_to_service()
        {
            var result = taskMapper.ConvertToService(contractTask);
            result.Should().BeOfType<ServiceTask>();
            result.Name.Should().Be(contractTask.Name);
            result.Id.Should().Be(contractTask.Id);
            result.IsCompleted.Should().Be(contractTask.IsCompleted);
        }
    }
}
