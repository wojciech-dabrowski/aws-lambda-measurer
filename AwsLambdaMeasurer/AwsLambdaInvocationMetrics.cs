using System;

namespace AwsLambdaMeasurer
{
    public class AwsLambdaInvocationMetrics
    {
        public AwsLambdaInvocationMetrics(TimeSpan duration, TimeSpan billedDuration, int maxMemoryUsed)
        {
            Duration = duration;
            BilledDuration = billedDuration;
            MaxMemoryUsed = maxMemoryUsed;
        }

        public TimeSpan Duration { get; }
        public TimeSpan BilledDuration { get; }
        public int MaxMemoryUsed { get; }
    }
}