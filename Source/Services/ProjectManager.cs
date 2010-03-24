using System;
using System.IO;
using Kata.Generators;
using Kata.Messages;

namespace Kata.Services
{
    class ProjectManager : Handles<EnvironmentReady>, Accepts<Config>
    {
        private string _kataName;

        public void Handle(EnvironmentReady message)
        {
            var projectPath = Path.Combine(message.EnvironmentPath, String.Format("Kata{0}.csproj", _kataName));
            var solutionPath = Path.Combine(message.EnvironmentPath, String.Format("Kata{0}.sln", _kataName));
            var codePath = Path.Combine(message.EnvironmentPath, String.Format("Kata{0}.cs", _kataName));


            File.WriteAllText(projectPath, new ProjectFileGenerator(_kataName).TransformText());
            File.WriteAllText(solutionPath, new SolutionFileGenerator(_kataName).TransformText());
            File.WriteAllText(codePath, new CodeGenerator(_kataName).TransformText());

            Events.Raise(new ProjectCreated
            {
                FullProjectPath = projectPath,
                FullSolutionPath = solutionPath
            });
        }

        public void Accept(Config item)
        {
            _kataName = item.KataName;
        }
    }
}
