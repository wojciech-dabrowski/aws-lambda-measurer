using System;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;

namespace AwsLambdaMeasurer
{
    public class MeasureService
    {
        private readonly MeasurerConfiguration _config;
        private readonly IAmazonLambda _lambdaClient;
        private readonly ILogger _logger;

        public MeasureService(MeasurerConfiguration config, IAmazonLambda lambdaClient, ILogger logger)
        {
            _config = config;
            _lambdaClient = lambdaClient;
            _logger = logger;
        }

        public async Task<MeasurementsSummary> PerformMeasurements()
        {
            var originalFunction = await _lambdaClient.GetFunctionAsync(_config.LambdaFunctionName);
            var originalFunctionMemorySize = originalFunction.Configuration.MemorySize;
            _logger.LogMessage($"Starting measurements for the function {_config.LambdaFunctionName}. " +
                               $"Original memory size: {originalFunctionMemorySize}");

            foreach (var memorySize in _config.MemoriesMeasured)
            {
                await UpdateFunctionMemorySize(memorySize);

                for (var invocationNumber = 1; invocationNumber < _config.InvocationsPerMemorySize; invocationNumber++)
                {
                    var request = new InvokeRequest
                    {
                        FunctionName = _config.LambdaFunctionName
                    };

                    var response = await _lambdaClient.InvokeAsync(request);

                    var metadata = response.ResponseMetadata.Metadata;
                }
            }

            _logger.LogMessage("Reverting original memory size...");
            await UpdateFunctionMemorySize(originalFunctionMemorySize);

            throw new NotImplementedException();
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