using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace FacebookInformationPoster
{
    public abstract class AsyncTimerAction : IDisposable, IAsyncTimerAction
    {
        private readonly HashSet<Timer> _timers
            = new HashSet<Timer>();

        private AsyncTimerActionOptions _options { get; init; }
            = new AsyncTimerActionOptions();

        private int _spanExecutionCount { get; set; }

        private static TimeSpan ModTimeSpan(TimeSpan timeSpan, TimeSpan mod)
        {
            while (timeSpan < TimeSpan.Zero)
                timeSpan += mod;
            while (timeSpan > mod)
                timeSpan -= mod;
            return timeSpan;
        }

        public abstract void OnConfiguring(AsyncTimerActionOptions options);

        public void Start()
        {
            OnConfiguring(_options);

            var oneDay = TimeSpan.FromDays(1);

            var mainTimer = new Timer(ModTimeSpan(_options.StartOffset - DateTime.Now.TimeOfDay, oneDay).TotalMilliseconds);
            if (_options.StartOffset == TimeSpan.Zero)
                mainTimer.Interval = _options.ActionInterval.TotalMilliseconds;
            mainTimer.Elapsed += (o, e) =>
            {
                if ((_options.Limit <= 0 || _spanExecutionCount++ < _options.Limit))
                    _ = Task.Run(() =>
                    {
                        Action();
                    });

                mainTimer.Interval = _options.ActionInterval.TotalMilliseconds;
                mainTimer.Start();
            };

            _timers.Add(mainTimer);

            if (_options.Limit > 0 && _options.LimitSpan > TimeSpan.Zero)
            {
                var limitTimer = new Timer(ModTimeSpan(_options.LimitSpan - DateTime.Now.TimeOfDay, oneDay).TotalMilliseconds);
                limitTimer.Elapsed += (o, e) =>
                {
                    _spanExecutionCount = 0;
                    limitTimer.Interval = _options.LimitSpan.TotalMilliseconds;
                    limitTimer.Start();
                };

                _timers.Add(limitTimer);
            }

            foreach (var timer in _timers)
                timer.Start();
        }

        public abstract void Action();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach (var timer in _timers)
                timer.Dispose();
        }
    }
}
