using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CordovaMudBlazor.Library
{
    public class Debouncer : IDisposable
    {
        public TimeSpan _ts;
        private Action _action;
        public HashSet<ManualResetEvent> _resets = new HashSet<ManualResetEvent>();
        public object _mutex = new object();

        public Action Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public Debouncer(TimeSpan timespan, Action action)
        {
            _ts = timespan;
            _action = action;
        }

        public void Invoke()
        {
            //_action();
            //return;
            var thisReset = new ManualResetEvent(false);

            lock (_mutex)
            {
                while (_resets.Count > 0)
                {
                    var otherReset = _resets.First();
                    _resets.Remove(otherReset);
                    otherReset.Set();
                }

                _resets.Add(thisReset);
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    if (!thisReset.WaitOne(_ts))
                    {
                        _action();
                    }
                }
                finally
                {
                    lock (_mutex)
                    {
                        using (thisReset)
                            _resets.Remove(thisReset);
                    }
                }
            });
        }

        public void Dispose()
        {
            lock (_mutex)
            {
                while (_resets.Count > 0)
                {
                    var reset = _resets.First();
                    _resets.Remove(reset);
                    reset.Set();
                }
            }
        }
    }
}
