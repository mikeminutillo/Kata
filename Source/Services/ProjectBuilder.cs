using System.Threading;
using System.Threading.Tasks;
using Kata.Messages;
using Microsoft.Build.Evaluation;

namespace Kata.Services
{
    class ProjectBuilder : Handles<ProjectCreated>, Handles<ProjectChanged>
    {
        Project _project;

        static int _building = -1;

        public void Handle(ProjectCreated message)
        {
            _project = new Project(message.FullProjectPath);
        }

        public void Handle(ProjectChanged message)
        {
            var x = Interlocked.CompareExchange(ref _building, 1, -1);

            if (x == 1)
                return;

            Task.Factory.StartNew(BuildNow);
        }

        private void BuildNow()
        {
            try
            {
                var succeeded = _project.Build();

                Events.Raise(new ProjectBuilt { Succeeded = succeeded });
            }
            finally
            {
                Interlocked.Exchange(ref _building, -1);
            }

        }
    }
}
