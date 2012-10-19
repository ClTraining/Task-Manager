using System;
using System.Collections.Generic;
using System.ComponentModel;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class ArgumentConverter<T>
    {
        public virtual T Convert(List<string> input)
        {
            var tc = TypeDescriptor.GetConverter(typeof (T));
            return (T) tc.ConvertFrom(input);
        }
    }

    public class ArgumentConverterTests
    {
        [Fact]
        public void should_get_add_task_arguments()
        {
            var tc = new ArgumentConverter<AddTaskArgs>();
            var args = tc.Convert(new List<string> {"123"});
            args.ShouldBeEquivalentTo(new AddTaskArgs {Name = "123"});
        }

        [Fact]
        public void should_get_add_task_with_date_arguments()
        {
            var tc = new ArgumentConverter<AddTaskArgs>();
            var args = tc.Convert(new List<string> { "task 1", "01-01-2012" });
            args.ShouldBeEquivalentTo(new AddTaskArgs { Name = "task 1", DueDate = DateTime.Parse("01-01-2012")});
        }

        [Fact]
        public void should_get_rename_task_arguments()
        {
            var tc = new ArgumentConverter<RenameTaskArgs>();
            var result = tc.Convert(new List<string> {"11", "13dsd"});
            result.ShouldBeEquivalentTo(new RenameTaskArgs {Id = 11, Name = "13dsd"});
        }

        [Fact]
        public void should_get_complete_command_arguments()
        {
            var tc = new ArgumentConverter<CompleteTaskArgs>();
            var result = tc.Convert(new List<string> {"14"});
            result.ShouldBeEquivalentTo(new CompleteTaskArgs {Id = 14});
        }

        [Fact]
        public void should_get_list_single_task_args()
        {
            var tc = new ArgumentConverter<ListArgs>();
            var result = tc.Convert(new List<string> {"3"});
            result.ShouldBeEquivalentTo(new ListArgs {Id = 3});
        }

        [Fact]
        public void should_get_list_all_task_args()
        {
            var tc = new ArgumentConverter<ListArgs>();
            var result = tc.Convert(new List<string>());
            result.ShouldBeEquivalentTo(new ListArgs {Id = null});
        }

        [Fact]
        public void should_convert_set_date_args()
        {
            var tc = new ArgumentConverter<SetDateArgs>();
            var result = tc.Convert(new List<string> {"9","10-10-2012"});
            result.ShouldBeEquivalentTo(new SetDateArgs { Id = 9, DueDate = DateTime.Parse("10-10-2012") });
        }
    }
}