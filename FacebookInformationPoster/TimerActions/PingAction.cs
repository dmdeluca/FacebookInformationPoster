using Serilog;
using System;

namespace FacebookInformationPoster
{
    public class PingAction : IScheduledTask
    {
        private readonly ILogger _writer;

        public PingAction(ILogger writer)
        {
            _writer = writer;
        }

        public string Name => "Ping";

        public void Execute()
        {
            _writer.Information("ping");
        }
    }
}
