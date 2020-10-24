using System;

namespace AwsLambdaMeasurer
{
    public class AwsLambdaLogResultParser : IAwsLambdaLogResultParser
    {
        public AwsLambdaInvocationMetrics GetMetricsFromLogResult(string base64LogResult)
        {
            throw new NotImplementedException();
        }
    }
}