using System;

namespace Onbox.ReactFactory.V5
{
    public static class ReactFactory
    {
        public static Debouncer Debouncer() => new Debouncer();
        public static Interval Interval(Action<int> action, int interval, int? maxRuns = null) => new Interval(action, interval, maxRuns);
        public static Interval Interval(Action action, int interval, int? maxRuns = null) => new Interval(action, interval, maxRuns);
    }
}
