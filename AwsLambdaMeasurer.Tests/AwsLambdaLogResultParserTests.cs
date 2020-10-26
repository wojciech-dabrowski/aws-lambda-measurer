using System;
using AwsLambdaMeasurer.Utils;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AwsLambdaMeasurer.Tests
{
    public class AwsLambdaLogResultParserTests
    {
        public static TheoryData<string, AwsLambdaInvocationMetrics> ProperInvocationExpectedResults =
            new TheoryData<string, AwsLambdaInvocationMetrics>
            {
                {
                    @"
START RequestId: bb723c46-7e0e-4c79-9dc6-e6303a081511 Version: $LATEST
END RequestId: bb723c46-7e0e-4c79-9dc6-e6303a081511
REPORT RequestId: bb723c46-7e0e-4c79-9dc6-e6303a081511	Duration: 1.25 ms	Billed Duration: 100 ms	Memory Size: 128 MB	Max Memory Used: 51 MB	Init Duration: 129.80 ms",
                    new AwsLambdaInvocationMetrics(
                        TimeSpan.FromMilliseconds(1.25),
                        TimeSpan.FromMilliseconds(100),
                        TimeSpan.FromMilliseconds(129.80),
                        51)
                },
                {
                    @"
START RequestId: bb723c46-7e0e-4c79-9dc6-e6303a081511 Version: $LATEST
END RequestId: bb723c46-7e0e-4c79-9dc6-e6303a081511
REPORT RequestId: bb723c46-7e0e-4c79-9dc6-e6303a081511	Duration: 200.81 ms	Billed Duration: 300 ms	Memory Size: 128 MB	Max Memory Used: 79 MB	Init Duration: 5.10 ms",
                    new AwsLambdaInvocationMetrics(
                        TimeSpan.FromMilliseconds(200.81),
                        TimeSpan.FromMilliseconds(300),
                        TimeSpan.FromMilliseconds(5.10),
                        79)
                },
                {
                    @"
START RequestId: b0efb1d6-da45-407c-b1ce-5b727b8d5014 Version: $LATEST
Drift detection has failed for the Stack with ID: arn:aws:cloudformation:eu-west-1:886763261740:stack/spam-stack12/23a4d980-0d6b-11eb-b9c5-0a9b71aae734 with reason: {""Summary"":""Failed to detect drift on resources [ServerlessRestApiDeployment47fc2d5f9d]"",""Failures"":[{""Resource"":""ServerlessRestApiDeployment47fc2d5f9d"",""FailureReason"":""Rate exceeded""}]}
END RequestId: b0efb1d6-da45-407c-b1ce-5b727b8d5014
REPORT RequestId: b0efb1d6-da45-407c-b1ce-5b727b8d5014	Duration: 141810.39 ms	Billed Duration: 141900 ms	Memory Size: 128 MB	Max Memory Used: 76 MB	Init Duration: 757.92 ms",
                    new AwsLambdaInvocationMetrics(
                        TimeSpan.FromMilliseconds(141810.39),
                        TimeSpan.FromMilliseconds(141900),
                        TimeSpan.FromMilliseconds(757.92),
                        76)
                }
            };

        [Theory]
        [MemberData(nameof(ProperInvocationExpectedResults))]
        public void GetMetricsFromLogResult_GivenProperLogResult_ShouldReturnCorrectMetrics(
            string logResult,
            AwsLambdaInvocationMetrics expectedMetrics)
        {
            // Given
            var logResultBase64 = logResult.ToBase64String();
            var parser = new AwsLambdaLogResultParser();

            // When
            var result = parser.GetMetricsFromLogResult(logResultBase64);

            // Then
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Duration.Should().Be(expectedMetrics.Duration);
                result.BilledDuration.Should().Be(expectedMetrics.BilledDuration);
                result.InitDuration.Should().Be(expectedMetrics.InitDuration);
                result.MaxMemoryUsed.Should().Be(expectedMetrics.MaxMemoryUsed);
            }
        }

        [Fact]
        public void GetMetricsFromLogResult_GivenInvalidLogResult_ShouldThrowException()
        {
            // Given
            const string invalidLog = "This is an invalid LogResult without metrics.";
            var invalidLogBase64 = invalidLog.ToBase64String();
            var parser = new AwsLambdaLogResultParser();

            // When
            Func<AwsLambdaInvocationMetrics> func = () => parser.GetMetricsFromLogResult(invalidLogBase64);

            // Then
            func.Should().ThrowExactly<CannotParseMetricsFromLogResultException>();
        }
    }
}