using System.Collections.Generic;

namespace AwsLambdaMeasurer
{
    public class MeasurementsSummary
    {
        public MeasurementsSummary(Dictionary<int, IReadOnlyList<AwsLambdaInvocationMetrics>> rawMetrics)
        {
            RawMetrics = rawMetrics;
        }

        public Dictionary<int, IReadOnlyList<AwsLambdaInvocationMetrics>> RawMetrics { get; }
    }
}