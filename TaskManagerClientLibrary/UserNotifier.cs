using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class UserNotifier
    {
        private readonly string address;

        public UserNotifier(string address)
        {
            this.address = address;
        }

        public string GenerateGreeting()
        {
            var sb = new StringBuilder();
            sb.Append("Hello " + Environment.UserName);
            sb.Append("\nServer address is: " + address.Split(new[] { '/', ':' })[3]);
            sb.Append("\nType \'?\' to see available commands");
            return sb.ToString();
        }
    }

    public class UserNotifierTester
    {
        [Fact]
        public void should_generate_message()
        {
            const string test = "blabla";
            var notifier = new UserNotifier(string.Format("1/1/1:{0}", test));

            var greeting = notifier.GenerateGreeting();

            greeting.Should().Be("Hello " + Environment.UserName + "\nServer address is: " + test + "\nType \'?\' to see available commands");
        }
    }
}
