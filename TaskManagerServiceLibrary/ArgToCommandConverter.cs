using System;
using AutoMapper;
using EntitiesLibrary.CommandArguments;
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
        public ArgToCommandConverter()
        {
            Mapper.CreateMap<ClearDateTaskArgs, ClearDateServiceCommand>();
            Mapper.CreateMap<CompleteTaskArgs, CompleteServiceCommand>();
            Mapper.CreateMap<RenameTaskArgs, RenameServiceCommand>();
            Mapper.CreateMap<SetDateTaskArgs, SetDateServiceCommand>();
            Mapper.CreateMap<IEditCommandArguments, IServiceCommand>().ConvertUsing<MapConverter<IEditCommandArguments, IServiceCommand>>();
        }

        public IServiceCommand GetServiceCommand(IEditCommandArguments args)
        {
            return Mapper.DynamicMap<IEditCommandArguments, IServiceCommand>(args);
        }
    }

    public class ArgToCommandConverterTests
    {
        [Fact]
        public void test1()
        {
            var args = new RenameTaskArgs{Id = 1, Name = "name"};
            var conv = new ArgToCommandConverter();
            var result = (RenameTaskArgs)conv.GetServiceCommand(args);
            Console.Out.WriteLine(result.Id + " " + result.Name);
        }
    }
}
