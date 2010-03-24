using System;
using Kata.Messages;

namespace Kata.Services
{
    class ConsoleNotifier : Handles<ProjectBuilt>, Handles<TestRunComplete>
    {
        public void Handle(ProjectBuilt message)
        {
            if(message.Succeeded)
                Console.WriteLine("Project Built Successfully");
            else
                Console.WriteLine("Project Built Unsuccessfully");
        }

        public void Handle(TestRunComplete message)
        {
            if(message.Failed > 0)
                Console.WriteLine("{0} out of {1} tests failed", message.Failed, message.Total);
            else
                Console.WriteLine("All tests passed");
        }
    }
}
