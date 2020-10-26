using System;

namespace AwsLambdaMeasurer
{
    public class AwsLambdaInvocationMetrics
    {
        public AwsLambdaInvocationMetrics(TimeSpan duration, TimeSpan billedDuration, TimeSpan initDuration,
            int maxMemoryUsed)
        {
            Duration = duration;
            BilledDuration = billedDuration;
            InitDuration = initDuration;
            MaxMemoryUsed = maxMemoryUsed;
        }

        public TimeSpan Duration { get; }
        public TimeSpan BilledDuration { get; }
        public TimeSpan InitDuration { get; }
        public int MaxMemoryUsed { get; }
    }
}