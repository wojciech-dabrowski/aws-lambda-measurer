using System;
using System.Text.RegularExpressions;
using AwsLambdaMeasurer.Utils;

namespace AwsLambdaMeasurer
{
    public class AwsLambdaLogResultParser : IAwsLambdaLogResultParser
    {
        public AwsLambdaInvocationMetrics GetMetricsFromLogResult(string base64LogResult)
        {
            var logResult = base64LogResult.DecodeBase64();

            try
            {
                var duration = GetTimeSpanFromMatchingString(logResult, @"Duration: ([0-9.]*)");
                var billedDuration = GetTimeSpanFromMatchingString(logResult, @"Billed Duration: ([0-9.]*)");
                var initDuration = GetTimeSpanFromMatchingString(logResult, @"Init Duration: ([0-9.]*)");

                var maxMemoryUsedString = ExtractNumber(Regex.Match(logResult, @"Max Memory Used: ([0-9.]*)").Value);
                var maxMemoryUsed = int.Parse(maxMemoryUsedString);

                return new AwsLambdaInvocationMetrics(duration, billedDuration, initDuration, maxMemoryUsed);
            }
            catch (Exception)
            {
                throw new CannotParseMetricsFromLogResultException(logResult);
            }
        }

        private static TimeSpan GetTimeSpanFromMatchingString(string inputText, string pattern)
        {
            var millisecondAsString = ExtractNumber(Regex.Match(inputText, pattern).Value);
            var timeSpan = TimeSpan.FromMilliseconds(double.Parse(millisecondAsString));
            return timeSpan;
        }

        private static string ExtractNumber(string inputText)
        {
            return Regex.Match(inputText, @"([0-9]*\.?[0-9]+)").Value;
        }
    }
}