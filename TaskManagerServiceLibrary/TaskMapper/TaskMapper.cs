using System;
using AutoMapper;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace TaskManagerServiceLibrary.TaskMapper
{
    public class TaskMapper : ITaskMapper
    {
        public TaskMapper()
        {
            Mapper.CreateMap<ServiceTask, ClientTask>();
            Mapper.CreateMap<ClientTask, ServiceTask>();
        }

        public ClientTask ConvertToClient(ServiceTask task)
        {
            return Mapper.DynamicMap<ServiceTask, ClientTask>(task);
        }

        public ServiceTask ConvertArgsToTask(AddTaskArgs args)
        {
            return new ServiceTask { Name = args.Name, DueDate = args.DueDate == null ? default(DateTime) : args.DueDate.Value };
        }
    }

    public class TaskMapperTests
    {
        private readonly ClientTask clientTask = new ClientTask
                                                     {
                                                         Id = 10,
                                                         Name = "service",
                                                         IsCompleted = true,
                                                         DueDate = default(DateTime)
                                                     };

        private readonly ServiceTask serviceTask = new ServiceTask
                                                       {
                                                           Id = 10,
                                                           Name = "service",
                                                           IsCompleted = true,
                                                           DueDate = default(DateTime)
                                                       };

        private readonly TaskMapper mapper = new TaskMapper();

        [Fact]
        public void contract_task_should_be_equivalent_to_service_task()
        {
            var res = mapper.ConvertToClient(serviceTask);
            res.ShouldBeEquivalentTo(clientTask);
        }

        [Fact]
        public void should_return_null_if_null_passed_convert_from_service()
        {
            var result = mapper.ConvertToClient(null);
            result.Should().BeNull();
        }
    }
}