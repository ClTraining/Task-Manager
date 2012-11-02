using System;
using AutoMapper;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Ninject;
using TaskManagerServiceLibrary.Commands;
using Xunit;

namespace TaskManagerServiceLibrary
{
    public interface IArgToCommandConverter
    {
        IServiceCommand GetServiceCommand(IEditCommandArguments args);
    }

    public class ArgToCommandConverter : IArgToCommandConverter
    {
        public ArgToCommandConverter(IKernel kernel)
        {
            Mapper.Initialize(map => map.ConstructServicesUsing(t => kernel.Get(t)));
            Mapper.CreateMap<ClearDateTaskArgs, ClearDateServiceCommand>()
                .ConstructUsing((ClearDateTaskArgs a) => kernel.Get<ClearDateServiceCommand>());

            Mapper.CreateMap<CompleteTaskArgs, CompleteServiceCommand>()
                .ConstructUsing((CompleteTaskArgs a) => kernel.Get<CompleteServiceCommand>());

            Mapper.CreateMap<RenameTaskArgs, RenameServiceCommand>()
                .ConstructUsing((RenameTaskArgs a) => kernel.Get<RenameServiceCommand>());

            Mapper.CreateMap<SetDateTaskArgs, SetDateServiceCommand>()
                .ConstructUsing((SetDateTaskArgs a) => kernel.Get<SetDateServiceCommand>());

            Mapper.CreateMap<IEditCommandArguments, IServiceCommand>().ConvertUsing<MapConverter<IEditCommandArguments, IServiceCommand>>();
        }

        public IServiceCommand GetServiceCommand(IEditCommandArguments args)
        {
            return Mapper.DynamicMap<IServiceCommand>(args);
        }
    }

    public class ArgToCommandConverterTests
    {
        readonly IKernel kernel = new StandardKernel();
        readonly ArgToCommandConverter converter;

        public ArgToCommandConverterTests()
        {
            kernel.Bind<ITodoList>().ToConstant(Substitute.For<ITodoList>());
            converter = new ArgToCommandConverter(kernel);
        }

        [Fact]
        public void should_convert_args_to_clear_date_command()
        {
            var args = new ClearDateTaskArgs {Id = 1};

            var result = (ClearDateServiceCommand) converter.GetServiceCommand(args);

            result.Id.Should().Be(1);
        }

        [Fact]
        public void should_convert_args_to_complete_command()
        {
            var args = new CompleteTaskArgs { Id = 1 };

            var result = (CompleteServiceCommand)converter.GetServiceCommand(args);

            result.Id.Should().Be(1);
        }

        [Fact]
        public void should_convert_args_to_rename_command()
        {
            var args = new RenameTaskArgs {Id = 1, Name = "task1"};

            var result = (RenameServiceCommand) converter.GetServiceCommand(args);

            result.Id.Should().Be(1);
            result.Name.Should().Be("task1");
        }

        [Fact]
        public void should_convert_args_to_set_date_command()
        {
            var args = new SetDateTaskArgs {Id = 1, DueDate = DateTime.Today};

            var result = (SetDateServiceCommand) converter.GetServiceCommand(args);

            result.Id.Should().Be(1);
            result.DueDate.Should().Be(DateTime.Today);
        }
    }
}
