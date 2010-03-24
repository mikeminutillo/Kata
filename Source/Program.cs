using System;
using System.Linq;
using Kata.Messages;

namespace Kata
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("USAGE: kata.exe <kata name>");
                Environment.Exit(-1);
            }

            var kataName = GetKataName(args.First());

            var config = new Config
            {
                KataName = kataName
            };

            using (var bootstrapper = new Bootstrapper())
            {
                bootstrapper.Start(config);
                Console.WriteLine("Kata {0} started.", kataName);
                Events.WaitFor<VisualStudioClosed>();
            }
        }

        private static string GetKataName(string arg)
        {
            return String.Format("{0}{1}", 
                arg.Substring(0, 1).ToUpper(),
                arg.Substring(1).ToLower());
        }
    }
}
