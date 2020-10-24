namespace AwsLambdaMeasurer
{
    public interface IAwsLambdaLogResultParser
    {
        AwsLambdaInvocationMetrics GetMetricsFromLogResult(string base64LogResult);
    }
}