using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Timers;

namespace FacebookInformationPoster
{
    public sealed class Scheduler : IDisposable, IScheduler
    {
        private readonly ILogger _logger;
        private readonly Timer _poll;

        public Scheduler(ILogger logger, IConfiguration configuration, IEnumerable<IScheduledTask> scheduledTasks)
        {
            _logger = logger;
            _poll = new Timer()
            {
                AutoReset = true,
                Interval = 1000 * 60
            };

            foreach (var task in scheduledTasks)
            {
                _poll.Elapsed += (o, e) =>
                {
                    var configuredCron = configuration
                        .GetSection("Crons")
                        .GetSection(task.Name);

                    if (!configuredCron.Exists())
                    {
                        _logger.Warning($"Action '{task.Name}' has no configured CRON.");
                        return;
                    }

                    var cron = new Cron(configuredCron.Value);

                    if (!cron.Matches(DateTime.Now))
                    {
                        return;
                    }

                    _logger.Information($"Triggering task {task.Name} with CRON [{cron}]");
                    try
                    {
                        task.Execute();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"Exception thrown by task '{task}'");
                    }
                    _logger.Information($"Task {task.Name} is done.");
                };
            }
        }

        public void StartPolling()
        {
            _poll.Start();
        }

        public void Dispose()
        {
            _poll.Dispose();
        }
    }

    //public abstract class AsyncTimerAction : IDisposable, IAsyncTimerAction
    //{
    //    private readonly HashSet<Timer> _timers
    //        = new HashSet<Timer>();

    //    private readonly IConfiguration _configuration;

    //    private AsyncTimerActionOptions _options { get; init; }
    //        = new AsyncTimerActionOptions();

    //    private int _spanExecutionCount { get; set; }

    //    private static TimeSpan ModTimeSpan(TimeSpan timeSpan, TimeSpan mod)
    //    {
    //        while (timeSpan < TimeSpan.Zero)
    //            timeSpan += mod;
    //        while (timeSpan > mod)
    //            timeSpan -= mod;
    //        return timeSpan;
    //    }

    //    public AsyncTimerAction(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    public virtual void OnConfiguring(AsyncTimerActionOptions options)
    //    {
    //        var values = _configuration.GetSection("Actions")
    //            .GetSection(GetType().Name.Replace("Action", ""));

    //        AsyncTimerActionOptions2 structOptions = new AsyncTimerActionOptions2();
    //        if (values.GetSection("Cron").Exists())
    //            structOptions.Cron = values.GetSection("Cron").Value;
    //    }

    //    public void Start()
    //    {
    //        OnConfiguring(_options);

    //        var oneDay = TimeSpan.FromDays(1);

    //        var mainTimer = new Timer(ModTimeSpan(_options.StartOffset - DateTime.Now.TimeOfDay, oneDay).TotalMilliseconds);
    //        if (_options.StartOffset == TimeSpan.Zero)
    //            mainTimer.Interval = _options.ActionInterval.TotalMilliseconds;
    //        mainTimer.Elapsed += (o, e) =>
    //        {
    //            if ((_options.Limit <= 0 || _spanExecutionCount++ < _options.Limit))
    //                _ = Task.Run(() =>
    //                {
    //                    Action();
    //                });

    //            mainTimer.Interval = _options.ActionInterval.TotalMilliseconds;
    //            mainTimer.Start();
    //        };

    //        _timers.Add(mainTimer);

    //        if (_options.Limit > 0 && _options.LimitSpan > TimeSpan.Zero)
    //        {
    //            var limitTimer = new Timer(ModTimeSpan(_options.LimitSpan - DateTime.Now.TimeOfDay, oneDay).TotalMilliseconds);
    //            limitTimer.Elapsed += (o, e) =>
    //            {
    //                _spanExecutionCount = 0;
    //                limitTimer.Interval = _options.LimitSpan.TotalMilliseconds;
    //                limitTimer.Start();
    //            };

    //            _timers.Add(limitTimer);
    //        }

    //        foreach (var timer in _timers)
    //            timer.Start();
    //    }

    //    public abstract void Action();

    //    public void Dispose()
    //    {
    //        GC.SuppressFinalize(this);
    //        foreach (var timer in _timers)
    //            timer.Dispose();
    //    }
    //}
}
