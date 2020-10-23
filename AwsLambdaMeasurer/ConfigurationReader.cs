using System;
using Microsoft.Extensions.Configuration;

namespace AwsLambdaMeasurer
{
    public class ConfigurationReader
    {
        private readonly IConfigurationRoot _config;

        public ConfigurationReader()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.local.json", true, true)
                .Build();
        }

        public MeasurerConfiguration ReadConfig()
        {
            var config = new MeasurerConfiguration();
            _config.Bind(config);
            return config;
        }
    }
}