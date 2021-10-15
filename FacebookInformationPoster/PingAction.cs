using Serilog;
using System;

namespace FacebookInformationPoster
{
    public class PingAction : AsyncTimerAction
    {
        private readonly ILogger _writer;

        public PingAction(ILogger writer)
        {
            _writer = writer;
        }

        public override void Action()
        {
            _writer.Information("ping");
        }

        public override void OnConfiguring(AsyncTimerActionOptions options)
        {
            options.ActionInterval = TimeSpan.FromMinutes(1) / 10;
        }
    }
}
