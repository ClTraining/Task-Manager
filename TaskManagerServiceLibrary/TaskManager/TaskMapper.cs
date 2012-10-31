using System;
using System.Collections.Generic;
using AutoMapper;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerServiceLibrary.TaskManager
{
    public class TaskMapper : ITaskMapper
    {
        public TaskMapper()
        {
            Mapper.CreateMap<ServiceTask, ClientPackage>();
            Mapper.CreateMap<ClientPackage, ServiceTask>();

            Mapper.CreateMap<ICommandArguments, ServiceTask>()
                .Include<IClearDateTaskArgs, ServiceTask>()
                .ForMember(s => s.DueDate, o => o.MapFrom(a => (a as IClearDateTaskArgs).Date))
                .Include<ICompleteTaskArgs, ServiceTask>()
                .ForMember(s => s.IsCompleted, o => o.MapFrom(a => (a as ICompleteTaskArgs).IsCompleted))
                .Include<IRenameTaskArgs, ServiceTask>()
                .ForMember(s => s.Name, o => o.MapFrom(a => (a as IRenameTaskArgs).Name))
                .Include<ISetDateTaskArgs, ServiceTask>()
                .ForMember(s => s.DueDate, o => o.MapFrom(a => (a as SetDateTaskArgs).DueDate));
        }

        public ClientPackage ConvertToContract(ServiceTask task)
        {
            return Mapper.Map<ServiceTask, ClientPackage>(task);
        }

        public ServiceTask Convert(ICommandArguments fromArgs, ServiceTask toTask)
        {
            return Mapper.Map(fromArgs, toTask);
        }
    }

    public class TaskMapperTests
    {
        private readonly ClientPackage contractTask = new ClientPackage { Id = 10, Name = "service", IsCompleted = true };
        private readonly ServiceTask serviceTask = new ServiceTask { Id = 10, Name = "service", IsCompleted = true };
        private readonly TaskMapper mapper = new TaskMapper();

        [Fact]
        public void contract_task_should_be_equivalent_to_service_task()
        {
            var res = mapper.ConvertToContract(serviceTask);
            res.ShouldBeEquivalentTo(contractTask);
        }

        [Fact]
        public void should_return_null_if_null_passed_convert_from_service()
        {
            var result = mapper.ConvertToContract(null);
            result.Should().BeNull();
        }

        [Fact]
        public void should_convert_to_servicetask()
        {
            var task = new ServiceTask { Name = "task1" };
            var renameTaskArgs = new RenameTaskArgs { Name = "12345" };

            var result = mapper.Convert(renameTaskArgs, task);

            result.Name.Should().Be("12345");
        }
    }
}