using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kata.Messages;
using Xunit;

namespace Kata.Services
{
    class XUnitRunner : Handles<ProjectBuilt>, Handles<ProjectCreated>, Accepts<Config>
    {
        private string _kataName;
        private string _assemblyPath;

        public void Handle(ProjectBuilt message)
        {
            if (message.Succeeded == false) return;

            using (var wrapper = new ExecutorWrapper(_assemblyPath, null, true))
            {
                var runner = new TestRunner(wrapper, new XUnitLogger());
                runner.RunAssembly();
            }
        }

        public void Handle(ProjectCreated message)
        {
            _assemblyPath = Path.Combine(new FileInfo(message.FullProjectPath).DirectoryName, String.Format(@"bin\Debug\Kata{0}.dll", _kataName));
        }

        public void Accept(Config item)
        {
            _kataName = item.KataName;
        }


        private class XUnitLogger : IRunnerLogger
        {
            private IList<String> _failures = new List<string>();

            public void AssemblyFinished(string assemblyFilename, int total, int failed, int skipped, double time)
            {
                Events.Raise(new TestRunComplete
                {
                    Total = total,
                    Skipped = skipped,
                    Failed = failed,
                    Failures = _failures.ToArray()
                });
            }

            public void AssemblyStart(string assemblyFilename, string configFilename, string xUnitVersion)
            {
                _failures.Clear();
            }

            public bool ClassFailed(string className, string exceptionType, string message, string stackTrace)
            {
                return true;
            }

            public void ExceptionThrown(string assemblyFilename, Exception exception)
            {
                
            }

            public void TestFailed(string name, string type, string method, double duration, string output, string exceptionType, string message, string stackTrace)
            {
                _failures.Add(String.Format("{0}: {1}", method, message));
            }

            public bool TestFinished(string name, string type, string method)
            {
                return true;
            }

            public void TestPassed(string name, string type, string method, double duration, string output)
            {
            }

            public void TestSkipped(string name, string type, string method, string reason)
            {
            }

            public bool TestStart(string name, string type, string method)
            {
                return true;
            }
        }

    }
}
