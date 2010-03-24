using System;
using System.IO;
using System.Reflection;
using Kata.Messages;

namespace Kata.Services
{
    class EnvironmentManager : Startable, Accepts<Config>, IDisposable
    {
        private readonly string _baseDir;

        private string _kataFolder; 

        public EnvironmentManager()
        {
            _baseDir = Environment.ExpandEnvironmentVariables(@"%TEMP%\Katas\");
        }

        public void Accept(Config item)
        {
            _kataFolder = Path.Combine(_baseDir, item.KataName);
        }

        public void Start()
        {
            CopyDLLs();

            var folderInfo = new DirectoryInfo(_kataFolder);
            if (!folderInfo.Exists)
                folderInfo.Create();

            Events.Raise(new EnvironmentReady { EnvironmentPath = _kataFolder });
        }

        private void CopyDLLs()
        {
            var libPath = Path.Combine(_baseDir, "Lib");
            var folderInfo = new DirectoryInfo(libPath);
            if (!folderInfo.Exists)
                folderInfo.Create();

            var currentPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            foreach (var file in currentPath.EnumerateFiles("*xunit*.dll"))
            {
                file.CopyTo(Path.Combine(libPath, file.Name), true);
            }
        }

        public void Dispose()
        {
            var folderInfo = new DirectoryInfo(_kataFolder);
            if (folderInfo.Exists)
                folderInfo.Delete(true);
        }
    }
}
