using System;
using System.IO.Abstractions;
using System.Linq;

namespace FacebookInformationPoster
{
    public class AsyncTimerActionOptions
    {
        public int Limit { get; set; }
        public TimeSpan ActionInterval { get; set; }
        public TimeSpan LimitSpan { get; set; }
        public TimeSpan StartOffset { get; set; }
    }
}
