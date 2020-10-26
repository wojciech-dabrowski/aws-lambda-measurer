using System;

namespace AwsLambdaMeasurer
{
    public class CannotParseMetricsFromLogResultException : Exception
    {
        public CannotParseMetricsFromLogResultException(string logResult) : base("Cannot parse metrics from LogResult.")
        {
            LogResult = logResult;
        }

        public string LogResult { get; }
    }
}