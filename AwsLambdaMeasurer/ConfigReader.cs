using System;
using Microsoft.Extensions.Configuration;

namespace AwsLambdaMeasurer
{
    public class ConfigReader
    {
        private IConfiguration _configuration;
        
        public ConfigReader()
        {
            
        }
        
        public MeasurerConfiguration ReadConfig()
        {
            throw new NotImplementedException();
        }
    }
}