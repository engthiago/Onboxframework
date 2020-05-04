using System;

namespace Onbox.Core.V6.ReactFactory
{
    /// <summary>
    /// Factory used to create reactive classes
    /// </summary>
    public static class ReactFactory
    {
        /// <summary>
        /// Onbox Debouncer runs an action after a particular time span has passed without another action is fired
        /// </summary>
        public static Debouncer Debouncer() => new Debouncer();

        /// <summary>
        /// Runs an action sequential times every specified interval of time and shows the number of times ran. It is possible to specify maximum runs as well
        /// </summary>
        public static Interval Interval(Action<int> action, int interval, int? maxRuns = null) => new Interval(action, interval, maxRuns);

        /// <summary>
        /// Runs an action sequential times every specified interval of time. It is possible to specify maximum runs as well
        /// </summary>
        public static Interval Interval(Action action, int interval, int? maxRuns = null) => new Interval(action, interval, maxRuns);
    }
}
