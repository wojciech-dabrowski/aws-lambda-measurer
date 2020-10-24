﻿using System;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Runtime.CredentialManagement;

namespace AwsLambdaMeasurer
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            var config = new ConfigurationReader().ReadConfig();
            var profileSource = new SharedCredentialsFile();
            profileSource.TryGetProfile(config.ProfileName, out var credentialProfile);
            var credentials = credentialProfile.GetAWSCredentials(profileSource);
            var lambdaClient = new AmazonLambdaClient(credentials, new AmazonLambdaConfig
            {
                Timeout = TimeSpan.FromMinutes(15),
                MaxErrorRetry = 0
            });

            var logger = new ConsoleLogger();
            var parser = new AwsLambdaLogResultParser();
            var measurer = new MeasureService(config, lambdaClient, logger, parser);
            var reportFactory = new ReportFactory();
            var reportWriter = new ReportWriter();

            var measurementsSummary = await measurer.PerformMeasurements();
            var csvReport = reportFactory.CreateCsvReport(measurementsSummary);
            await reportWriter.SaveReport(csvReport);
        }
    }
}