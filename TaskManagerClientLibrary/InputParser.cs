using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class InputParser
    {
        private const string Pattern = "(?:^|,)(\"(?:[^\"]+)*\"|\'(?:[^\']+)*\'|[^,]*)";
        private readonly char[] splitCommandAndArgumentsChars = new[] {' ', '\t'};
        private readonly char[] trimChars = new[] {'\'', '\"', ' ', '\t', '\n'};

        public virtual List<string> Parse(string input, int commandWordsCount = 1)
        {
            var inputArr = input.Split(splitCommandAndArgumentsChars, commandWordsCount + 1);
            var arguments = new List<string> {inputArr[commandWordsCount - 1]};

            if (inputArr.Count() > commandWordsCount)
            {
                var argumentsStr = inputArr[commandWordsCount];
                var exp = new Regex(Pattern);
                arguments.AddRange(exp.Split(argumentsStr));
            }

            var parcedInput = arguments.Where(s => !String.IsNullOrEmpty(s)).Select(s => s.Trim(trimChars)).ToList();
            return parcedInput;
        }
    }

    public class InputParserTests
    {
        private readonly InputParser parser = new InputParser();

        [Fact]
        public void should_split_one_argumet_with_double_quotes()
        {
            const string input = "add \"some task\"";
            var result = parser.Parse(input);
            result.ShouldBeEquivalentTo(new List<string> {"add", "some task"});
        }

        [Fact]
        public void should_split_help_command_()
        {
            const string input = "? \"some task\"";
            var result = parser.Parse(input);
            result.ShouldBeEquivalentTo(new List<string> {"?", "some task"});
        }

        [Fact]
        public void should_split_help_command_without_arguments()
        {
            const string input = "?";
            var result = parser.Parse(input);
            result.ShouldBeEquivalentTo(new List<string> {"?"});
        }

        [Fact]
        public void should_split_two_argumet_with_double_quotes()
        {
            const string input = "add \"some task\" 10-10-2012";
            var result = parser.Parse(input);
            result.ShouldBeEquivalentTo(new List<string> {"add", "some task", "10-10-2012"});
        }

        [Fact]
        public void should_split_two_argumet_with_single_quotes()
        {
            const string input = "add \'some task\' 10-10-2012";
            var result = parser.Parse(input);
            result.ShouldBeEquivalentTo(new List<string> {"add", "some task", "10-10-2012"});
        }

        [Fact]
        public void should_split_two_argumet_without_quotes()
        {
            const string input = "add some task, 10-10-2012";
            var result = parser.Parse(input);
            result.ShouldBeEquivalentTo(new List<string> {"add", "some task", "10-10-2012"});
        }

        [Fact]
        public void should_split_two_argumet_without_quotes_and_trim_extra_spaces()
        {
            const string input = "add some task   ,\t 10-10-2012\t\n";
            var result = parser.Parse(input);
            result.ShouldBeEquivalentTo(new List<string> {"add", "some task", "10-10-2012"});
        }
    }
}