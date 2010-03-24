using System;
using System.Linq;

namespace Kata
{
    class Bootstrapper : IDisposable
    {
        private IDisposable[] _disposables;

        public void Start<T>(T rootInfo)
        {
            var services = GetType().Assembly.GetTypes()
                                    .Where(x => x.Namespace.EndsWith("Services"))
                                    .Select(x => Activator.CreateInstance(x))
                                    .ToList();

            foreach (var service in services)
                Events.AddListener(service);

            foreach (var service in services.OfType<Accepts<T>>())
                service.Accept(rootInfo);
            
            foreach (var startable in services.OfType<Startable>())
                startable.Start();

            _disposables = services.OfType<IDisposable>().ToArray();
        }

        void IDisposable.Dispose()
        {
            if (_disposables == null)
                return;
            foreach (var disposable in _disposables)
            {
                if (disposable != null)
                    disposable.Dispose();
            }

            _disposables = null;
        }
    }
}
