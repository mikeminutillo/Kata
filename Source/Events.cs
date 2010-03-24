using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Kata
{
    static class Events
    {
        private static IList<Delegate> _actions = new List<Delegate>();
        private static IList<object> _listeners = new List<object>();
        private static object _lockObj = new object();

        public static void Raise<T>(T message) where T : Message
        {
            Action<T>[] actions;
            lock (_lockObj)
                actions = _actions.OfType<Action<T>>().ToArray();

            foreach (var action in actions)
                action(message);

            foreach (var listener in _listeners.OfType<Handles<T>>())
                listener.Handle(message);
        }

        public static void Raise<T>() where T : Message, new()
        {
            Raise(new T());
        }

        public static void AddListener(object listener)
        {
            _listeners.Add(listener);
        }

        public static void When<T>(Action<T> action)
        {
            lock (_lockObj)
                _actions.Add(action);
        }

        public static T WaitFor<T>() where T : Message
        {
            T returnValue = default(T);
            var resetEvent = new ManualResetEvent(false);
            Action<T> eventListener = message =>
            {
                returnValue = message;
                resetEvent.Set();
            };

            When<T>(eventListener);

            resetEvent.WaitOne();

            lock (_lockObj)
                _actions.Remove(eventListener);

            return returnValue;
        }
    }
}
