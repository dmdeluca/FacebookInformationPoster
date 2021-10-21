using System;
using System.Collections.Generic;
using System.Linq;

namespace FacebookInformationPoster
{
    public class Cron
    {
        const int MINUTE = 0;
        const int HOUR = 1;
        const int DAY = 2;
        const int MONTH = 3;
        const int WEEKDAY = 4;
        const int NUM_PARTS = 5;
        const string RANGE_SEPARATOR = "-";
        const string ITEM_SEPARATOR = ",";
        const string ANYTHING = "*";
        readonly Func<int, bool>[] _parts;
        private readonly string _cron;

        public Cron(string cron)
        {
            _parts = new Func<int, bool>[NUM_PARTS];

            var parts = cron.Split(" ").ToArray();
            if (parts.Length != NUM_PARTS)
                throw new ArgumentException("CRON string must have five parts.");

            for (int i = 0; i < _parts.Length; i++)
                _parts[i] = ParseCronPart(parts[i]);
            _cron = cron;
        }

        public bool Matches(DateTime dateTime)
        {
            return _parts[MINUTE](dateTime.Minute)
                && _parts[HOUR](dateTime.Hour)
                && _parts[DAY](dateTime.Day)
                && _parts[MONTH](dateTime.Month)
                && _parts[WEEKDAY]((int)dateTime.DayOfWeek);
        }


        public override string ToString()
        {
            return _cron;
        }

        private static Func<int, bool> ParseCronPart(string x)
        {
            if (x == ANYTHING)
                return i => true;

            var set = x.Split(ITEM_SEPARATOR)
                .SelectMany(ParseCronSubpart)
                .ToHashSet();

            return i => set.Contains(i);
        }

        private static IEnumerable<int> ParseCronSubpart(string y)
        {
            if (!y.Contains(RANGE_SEPARATOR))
                return new[] { int.Parse(y) };

            var range = y.Split(RANGE_SEPARATOR)
                .Select(int.Parse)
                .ToArray();

            int start = range[0];
            int count = range[1] - range[0];
            return Enumerable.Range(start, count);
        }

    }
}
