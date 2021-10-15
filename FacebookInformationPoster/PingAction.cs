using System;

namespace FacebookInformationPoster
{
    public class PingAction : AsyncTimerAction
    {
        private readonly IWriter _writer;

        public PingAction(IWriter writer)
        {
            _writer = writer;
        }

        public override void Action()
        {
            _writer.Log("ping");
        }

        public override void OnConfiguring(AsyncTimerActionOptions options)
        {
            options.ActionInterval = TimeSpan.FromMinutes(1) / 10;
        }
    }
}
