using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace TaskConsoleClient.UI
{
    public class CommandHelper
    {
        private static readonly List<string> commandPatterns = new List<string>
                                      {
                                          @"^(add)\s",
                                          @"^(list)$",
                                          @"^(list\s)\d+$",
                                          @"^(complete)\s\d+$"
                                      };


        public static bool Check(string text)
        {
            var result = true;
            if (!IsCommandCorrect(text))
            {
                result = false;
                Console.WriteLine("Command is not supported");
            }

            return result;
        }

        public static bool IsCommandCorrect(string text)
        {
            
            var regexes = commandPatterns.Select(x => new Regex(x)).ToList();
            return regexes.Any(regex => regex.IsMatch(text));
        }

        public static string GetCommand(string text)
        {
                      
            var regexes = commandPatterns.Select(x => new Regex(x)).ToList();
            var regex = regexes.FirstOrDefault(x => x.IsMatch(text));
            var match = regex.Match(text);
            Group group = null;
            if (match.Success)
                group = match.Groups[1];

            return group.ToString();
        }

    }

    public class ComandCheckerTester
    {
        [Fact]
        public void should_check_the_correctness_of_command()
        {
            // act
            var result = CommandHelper.Check("add asdasdasdasdasdasdasdasdasd");

            //assert
            result.Should().Be(true);
        }

        [Fact]
        public void should_extract_command_from_input()
        {
            // act
            var result = CommandHelper.GetCommand("list");

            // assert
            result.Should().BeEquivalentTo("list");
        }

        [Fact]
        public void should_print_command_is_not_correct_if_for_wrong_command()
        {
            // arrange
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            // act
            CommandHelper.Check("adadadadadadadadadad");

            // assert
            sb.ToString().Should().BeEquivalentTo("Command is not supported\r\n");
        }
    }
}
