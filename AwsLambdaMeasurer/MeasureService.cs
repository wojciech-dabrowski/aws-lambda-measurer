using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;

namespace AwsLambdaMeasurer
{
    public class MeasureService
    {
        private readonly IAwsLambdaLogResultParser _awsLambdaLogResultParser;
        private readonly MeasurerConfiguration _config;
        private readonly IAmazonLambda _lambdaClient;
        private readonly ILogger _logger;

        public MeasureService(MeasurerConfiguration config, IAmazonLambda lambdaClient, ILogger logger,
            IAwsLambdaLogResultParser awsLambdaLogResultParser)
        {
            _config = config;
            _lambdaClient = lambdaClient;
            _logger = logger;
            _awsLambdaLogResultParser = awsLambdaLogResultParser;
        }

        public async Task<MeasurementsSummary> PerformMeasurements()
        {
            var originalFunction = await _lambdaClient.GetFunctionAsync(_config.LambdaFunctionName);
            var originalFunctionMemorySize = originalFunction.Configuration.MemorySize;
            _logger.LogMessage($"Starting measurements for the function {_config.LambdaFunctionName}. " +
                               $"Original memory size: {originalFunctionMemorySize}");

            var measurementsResults = new Dictionary<int, IReadOnlyList<AwsLambdaInvocationMetrics>>();

            foreach (var memorySize in _config.MemoriesMeasured)
            {
                await UpdateFunctionMemorySize(memorySize);

                var optionMetrics = new List<AwsLambdaInvocationMetrics>();
                for (var invocationNumber = 1; invocationNumber < _config.InvocationsPerMemorySize; invocationNumber++)
                {
                    var request = new InvokeRequest
                    {
                        FunctionName = _config.LambdaFunctionName,
                        LogType = LogType.Tail,
                        InvocationType = InvocationType.RequestResponse
                    };

                    var response = await _lambdaClient.InvokeAsync(request);
                    optionMetrics.Add(_awsLambdaLogResultParser.GetMetricsFromLogResult(response.LogResult));
                }

                measurementsResults.Add(memorySize, optionMetrics);
            }

            _logger.LogMessage("Reverting original memory size...");
            await UpdateFunctionMemorySize(originalFunctionMemorySize);

            return new MeasurementsSummary(measurementsResults);
        }

        private async Task UpdateFunctionMemorySize(int memorySize)
        {
            var updateFunctionRequest = new UpdateFunctionConfigurationRequest
            {
                FunctionName = _config.LambdaFunctionName,
                MemorySize = memorySize
            };

            await _lambdaClient.UpdateFunctionConfigurationAsync(updateFunctionRequest);
        }
    }
}