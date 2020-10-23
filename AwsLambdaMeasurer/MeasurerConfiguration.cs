using System.Collections.Generic;

namespace AwsLambdaMeasurer
{
    public class MeasurerConfiguration
    {
        public string LambdaFunctionName { get; set; }
        public string ProfileName { get; set; }
        public IEnumerable<int> MemoriesMeasured { get; set; }
    }
}