using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Kata.Messages;

namespace Kata.Services
{
    class VisualStudioLauncher : Handles<ProjectCreated>
    {
        private string _solutionPath;

        public void Handle(ProjectCreated message)
        {
            _solutionPath = message.FullSolutionPath;

            Task.Factory.StartNew(RunVisualStudio);
        }

        private void RunVisualStudio()
        {
            var visualStudioPath = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Microsoft Visual Studio 10.0\Common7\IDE\Devenv.exe");

            var startInfo = new ProcessStartInfo(visualStudioPath, _solutionPath)
            {
                CreateNoWindow = true
            };


            var process = Process.Start(startInfo);
            process.WaitForExit();

            Events.Raise<VisualStudioClosed>();
        }
    }
}
