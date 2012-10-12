using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerClientLibrary
{
    public class UserNotifier
    {
        private readonly string address;

        public UserNotifier(string address)
        {
            this.address = address;
        }
        
        public void Greet()
        {
            Console.WriteLine("Hello " + Environment.UserName);
            Console.WriteLine("\nServer address is: " + address.Split(new[] { '/', ':' })[3] );
            Console.WriteLine("Type \'?\' to see available commands");
        }
    }
}
