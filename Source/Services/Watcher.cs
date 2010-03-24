using System;
using System.IO;
using Kata.Messages;

namespace Kata.Services
{
    class Watcher : Handles<ProjectCreated>, IDisposable
    {
        private FileSystemWatcher _watcher;

        public void Handle(ProjectCreated message)
        {
            _watcher = new FileSystemWatcher(new FileInfo(message.FullProjectPath).DirectoryName, "*.cs");
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.IncludeSubdirectories = true;
            _watcher.Changed += Changed;
            _watcher.Created += Changed;
            _watcher.EnableRaisingEvents = true;
        }

        private void Changed(object sender, FileSystemEventArgs args)
        {
           Events.Raise<ProjectChanged>();
        }

        public void Dispose()
        {
            if (_watcher != null)
                _watcher.Dispose();
        }
    }
}
